using ArcusTel.PortalClientes.Api.Data;
using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Enum;
using ArcusTel.PortalClientes.Api.Interface;
using ArcusTel.PortalClientes.Api.Models;

namespace ArcusTel.PortalClientes.Api.Service;

public class DidOrchestrator : IDidOrchestrator
{
    private readonly IPartnerClient _brClient;
    private readonly IPartnerClient _wtClient;
    private readonly AppDbContext _db;
    private readonly ILogger<DidOrchestrator> _log;
    private readonly IMapper _mapper; // optional AutoMapper or manual mapping

    public DidOrchestrator(IServiceProvider sp, AppDbContext db, ILogger<DidOrchestrator> log)
    {
        // resolve by partner
        _brClient = sp.GetRequiredService<PartnerBrasilClient>();
        _wtClient = sp.GetRequiredService<WorldTelClient>();
        _db = db;
        _log = log;
    }

    public async Task<NormalizedActivationResponse> ActivateAsync(string e164Number, long userId, CancellationToken ct = default)
    {
        var partner = e164Number.StartsWith("+55") ? PartnerId.PartnerBrasil : PartnerId.WorldTel;
        IPartnerClient client = partner == PartnerId.PartnerBrasil ? _brClient : _wtClient;

        // Persist initial DidActivationRequest
        var request = new DidActivationRequest
        {
            DidNumber = e164Number,
            PrefixCode = ExtractPrefix(e164Number),
            PartnerId = partner,
            RequestDate = DateTimeOffset.UtcNow,
            CurrentStatus = DidStatus.Pending,
            LastPartnerRawStatus = null!,
            UserId = userId
        };
        _db.TB_DidActivationRequest.Add(request);
        await _db.SaveChangesAsync(ct);

        // Call partner
        var (rawResponse, parsed) = await client.ActivateDidAsync(e164Number, "portal", ct);

        // Map partner status -> internal status
        var partnerStatus = ExtractPartnerStatus(parsed);
        var normalized = MapToNormalized(partner, parsed, partnerStatus);

        // Persist PartnerResponseLog
        var log = new PartnerResponseLog
        {
            RequestId = request.Id,
            Timestamp = DateTimeOffset.UtcNow,
            RawResponsePayload = rawResponse,
            PartnerStatus = partnerStatus,
            NormalizerOutputStatus = normalized.Status,
            PartnerDetailMessage = normalized.DetailMessage ?? string.Empty
        };
        _db.TB_PartnerResponseLog.Add(log);

        // Update request current status & last raw status
        request.CurrentStatus = normalized.Status;
        request.LastPartnerRawStatus = partnerStatus;
        await _db.SaveChangesAsync(ct);

        return normalized;
    }

    public async Task<NormalizedActivationResponse> GetStatusAsync(long requestId, CancellationToken ct = default)
    {
        var req = await _db.TB_DidActivationRequest.FindAsync(new object[] { requestId }, ct);
        if (req == null) throw new KeyNotFoundException("Request not found");

        // Optionally query partner for latest state
        IPartnerClient client = req.PartnerId == PartnerId.PartnerBrasil ? _brClient : _wtClient;
        (string raw, object parsed) = await client.GetStatusAsync(req.DidNumber, ct);

        var partnerStatus = ExtractPartnerStatus(parsed);
        var normalized = MapToNormalized(req.PartnerId, parsed, partnerStatus);

        // persist log & update
        var log = new PartnerResponseLog
        {
            RequestId = req.Id,
            Timestamp = DateTimeOffset.UtcNow,
            RawResponsePayload = raw,
            PartnerStatus = partnerStatus,
            NormalizerOutputStatus = normalized.Status,
            PartnerDetailMessage = normalized.DetailMessage ?? ""
        };
        _db.TB_PartnerResponseLog.Add(log);

        req.CurrentStatus = normalized.Status;
        req.LastPartnerRawStatus = partnerStatus;
        await _db.SaveChangesAsync(ct);

        return normalized;
    }
}
