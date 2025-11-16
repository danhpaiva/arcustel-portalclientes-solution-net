using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.Helpers;

public static class MapToNormalizedHelper
{
    public static NormalizedDidResponse FromPartnerBrasil(PartnerBrasilDidResponse partner)
    {
        var mapper = new PartnerStatusHelper();
        var didStatus = mapper.Map(PartnerId.BrasilConnect, partner.Status);

        return new NormalizedDidResponse
        {
            DidNumber = partner.DidNumber,
            Partner = "PartnerBrasil",
            Status = StatusConverter.ToNormalized(didStatus),
            ErrorMessage = partner.ErrorMessage,
            CreatedAt = partner.CreatedAt
        };
    }

    public static NormalizedDidResponse FromWorldTel(WorldTelDidResponse partner)
    {
        var mapper = new PartnerStatusHelper();
        var didStatus = mapper.Map(PartnerId.WorldTel, partner.Status);

        return new NormalizedDidResponse
        {
            DidNumber = partner.E164Number,
            Partner = "WorldTel",
            Status = StatusConverter.ToNormalized(didStatus),
            ErrorMessage = null,
            CreatedAt = partner.CreatedAt
        };
    }
}
