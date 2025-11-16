using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.Helpers;

public static class PartnerStatusHelper
{
    /// <summary>
    /// Extrai o status normalizado baseado no parceiro.
    /// </summary>
    public static NormalizedStatus ExtractPartnerStatus(string partner, int rawStatus)
    {
        return partner switch
        {
            "PartnerBrasil" => MapPartnerBrasil(rawStatus),
            "WorldTel" => MapWorldTel(rawStatus),
            _ => NormalizedStatus.Failed
        };
    }

    private static NormalizedStatus MapPartnerBrasil(int raw)
    {
        return raw switch
        {
            0 => NormalizedStatus.Pending,
            1 => NormalizedStatus.Active,
            2 => NormalizedStatus.Failed,
            _ => NormalizedStatus.Failed
        };
    }

    private static NormalizedStatus MapWorldTel(int raw)
    {
        return raw switch
        {
            0 => NormalizedStatus.Pending,
            1 => NormalizedStatus.Active,
            2 => NormalizedStatus.Failed,
            _ => NormalizedStatus.Failed
        };
    }
}

