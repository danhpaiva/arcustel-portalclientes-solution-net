using ArcusTel.PortalClientes.Api.Data;
using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Enum;
using ArcusTel.PortalClientes.Api.Interface;
using ArcusTel.PortalClientes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ArcusTel.PortalClientes.Api.Service;

public class DidOrchestrator : IDidOrchestrator
{
    private readonly AppDbContext _db;
    private readonly IPartnerBrasilClient _brClient;
    private readonly IWorldTelClient _wtClient;
    private readonly IPartnerStatusHelper _statusHelper;
    private readonly IPrefixHelper _prefixHelper;
    private readonly IDidNormalizer _normalizer;

    public DidOrchestrator(
        AppDbContext db,
        IPartnerBrasilClient brClient,
        IWorldTelClient wtClient,
        IPartnerStatusHelper statusHelper,
        IPrefixHelper prefixHelper,
        IDidNormalizer normalizer)
    {
        _db = db;
        _brClient = brClient;
        _wtClient = wtClient;
        _statusHelper = statusHelper;
        _prefixHelper = prefixHelper;
        _normalizer = normalizer;
    }

    public async Task<NormalizedActivationResponse> ActivateAsync(string e164Number, long userId, CancellationToken ct = default)
    {
        var partner = _prefixHelper.IsBrazilian(e164Number) ? PartnerId.BrasilConnect : PartnerId.WorldTel;

        var request = new DidActivationRequest
        {
            DidNumber = e164Number,
            PrefixCode = _prefixHelper.ExtractPrefix(e164Number),
            PartnerId = partner,
            RequestDate = DateTimeOffset.UtcNow,
            CurrentStatus = DidStatus.Pending,
            UserId = userId,
            LastPartnerRawStatus = "Pending"
        };


        _db.TB_DidActivationRequest.Add(request);
        await _db.SaveChangesAsync(ct);

        if (partner == PartnerId.BrasilConnect)
        {
            var partnerResp = await _brClient.ActivateDidAsync(e164Number, userId.ToString(), ct);
            if (partnerResp == null) throw new Exception("PartnerBrasil returned null");

            var normalized = _normalizer.FromPartnerBrasil(partnerResp);
            var mapped = _statusHelper.Map(PartnerId.BrasilConnect, partnerResp.Status);

            var log = new PartnerResponseLog
            {
                RequestId = request.Id,
                Timestamp = DateTimeOffset.UtcNow,
                RawResponsePayload = System.Text.Json.JsonSerializer.Serialize(partnerResp),
                PartnerStatus = partnerResp.Status.ToString(),
                NormalizerOutputStatus = mapped,
                PartnerDetailMessage = partnerResp.ErrorMessage ?? string.Empty
            };
            _db.TB_PartnerResponseLog.Add(log);

            request.CurrentStatus = mapped;
            request.LastPartnerRawStatus = partnerResp.Status.ToString();
            await _db.SaveChangesAsync(ct);

            return normalized;
        }
        else
        {
            var partnerResp = await _wtClient.ActivateDidAsync(e164Number, userId.ToString(), ct);
            if (partnerResp == null) throw new Exception("WorldTel returned null");

            var normalized = _normalizer.FromWorldTel(partnerResp);
            var mapped = _statusHelper.Map(PartnerId.WorldTel, partnerResp.Status);

            var log = new PartnerResponseLog
            {
                RequestId = request.Id,
                Timestamp = DateTimeOffset.UtcNow,
                RawResponsePayload = System.Text.Json.JsonSerializer.Serialize(partnerResp),
                PartnerStatus = partnerResp.Status.ToString(),
                NormalizerOutputStatus = mapped,
                PartnerDetailMessage = string.Empty
            };
            _db.TB_PartnerResponseLog.Add(log);

            request.CurrentStatus = mapped;
            request.LastPartnerRawStatus = partnerResp.Status.ToString();
            await _db.SaveChangesAsync(ct);

            return normalized;
        }
    }

    public async Task<NormalizedActivationResponse> GetStatusAsync(long requestId, CancellationToken ct = default)
    {
        var req = await _db.TB_DidActivationRequest.FindAsync(new object[] { requestId }, ct);
        if (req == null) throw new KeyNotFoundException("Request not found");

        if (req.PartnerId == PartnerId.BrasilConnect)
        {
            var partnerResp = await _brClient.GetStatusByNumberAsync(req.DidNumber, ct);
            if (partnerResp == null) throw new Exception("PartnerBrasil get-status returned null");

            var normalized = _normalizer.FromPartnerBrasil(partnerResp);
            var mapped = _statusHelper.Map(PartnerId.BrasilConnect, partnerResp.Status);

            var log = new PartnerResponseLog
            {
                RequestId = req.Id,
                Timestamp = DateTimeOffset.UtcNow,
                RawResponsePayload = System.Text.Json.JsonSerializer.Serialize(partnerResp),
                PartnerStatus = partnerResp.Status.ToString(),
                NormalizerOutputStatus = mapped,
                PartnerDetailMessage = partnerResp.ErrorMessage ?? string.Empty
            };
            _db.TB_PartnerResponseLog.Add(log);

            req.CurrentStatus = mapped;
            req.LastPartnerRawStatus = partnerResp.Status.ToString();
            await _db.SaveChangesAsync(ct);

            return normalized;
        }
        else
        {
            var partnerResp = await _wtClient.GetStatusByDidIdAsync(req.LastPartnerRawStatus ?? req.DidNumber, ct);
            if (partnerResp == null) throw new Exception("WorldTel get-status returned null");

            var normalized = _normalizer.FromWorldTel(partnerResp);
            var mapped = _statusHelper.Map(PartnerId.WorldTel, partnerResp.Status);

            var log = new PartnerResponseLog
            {
                RequestId = req.Id,
                Timestamp = DateTimeOffset.UtcNow,
                RawResponsePayload = System.Text.Json.JsonSerializer.Serialize(partnerResp),
                PartnerStatus = partnerResp.Status.ToString(),
                NormalizerOutputStatus = mapped,
                PartnerDetailMessage = string.Empty
            };
            _db.TB_PartnerResponseLog.Add(log);

            req.CurrentStatus = mapped;
            req.LastPartnerRawStatus = partnerResp.DidId ?? partnerResp.E164Number;
            await _db.SaveChangesAsync(ct);

            return normalized;
        }
    }
}