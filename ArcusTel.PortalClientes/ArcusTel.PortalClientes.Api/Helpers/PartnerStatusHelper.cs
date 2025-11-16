using ArcusTel.PortalClientes.Api.Enum;
using ArcusTel.PortalClientes.Api.Interface;

namespace ArcusTel.PortalClientes.Api.Helpers;

public class PartnerStatusHelper : IPartnerStatusHelper
{
    public DidStatus Map(PartnerId partner, string partnerExternalStatus)
    {
        if (int.TryParse(partnerExternalStatus, out var code))
            return Map(partner, code);

        return DidStatus.CustomerValidationFailure;
    }

    public DidStatus Map(PartnerId partner, int partnerStatusCode)
    {
        return partner switch
        {
            PartnerId.BrasilConnect => partnerStatusCode switch
            {
                0 => DidStatus.Pending,
                1 => DidStatus.Active,
                2 => DidStatus.PartnerFailure,
                _ => DidStatus.CustomerValidationFailure
            },
            PartnerId.WorldTel => partnerStatusCode switch
            {
                0 => DidStatus.Pending,
                1 => DidStatus.Active,
                2 => DidStatus.CustomerValidationFailure,
                _ => DidStatus.CustomerValidationFailure
            },
            _ => DidStatus.CustomerValidationFailure
        };
    }

    public static string ExtractPartnerStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return "Indisponível";

        status = status.Trim().ToLowerInvariant();

        return status switch
        {
            "ok" => "Operacional",
            "online" => "Operacional",
            "ativo" => "Operacional",
            "success" => "Operacional",
            "erro" => "Erro",
            "error" => "Erro",
            "offline" => "Offline",
            _ => status
        };
    }
}
