using ArcusTel.PortalClientes.Api.DTO;

namespace ArcusTel.PortalClientes.Api.Helpers;

public static class MapToNormalizedHelper
{
    public static NormalizedDidResponse FromPartnerBrasil(PartnerBrasilDidResponse partner)
    {
        return new NormalizedDidResponse
        {
            DidNumber = partner.DidNumber,
            Partner = "PartnerBrasil",
            Status = PartnerStatusHelper.ExtractPartnerStatus("PartnerBrasil", partner.Status),
            ErrorMessage = partner.ErrorMessage,
            CreatedAt = partner.CreatedAt
        };
    }

    public static NormalizedDidResponse FromWorldTel(WorldTelDidResponse partner)
    {
        return new NormalizedDidResponse
        {
            DidNumber = partner.E164Number,
            Partner = "WorldTel",
            Status = PartnerStatusHelper.ExtractPartnerStatus("WorldTel", partner.Status),
            ErrorMessage = null,
            CreatedAt = partner.CreatedAt
        };
    }
}

