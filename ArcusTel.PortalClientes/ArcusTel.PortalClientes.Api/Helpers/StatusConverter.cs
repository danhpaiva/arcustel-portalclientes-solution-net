using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.Helpers;

public static class StatusConverter
{
    public static NormalizedStatus ToNormalized(DidStatus status)
    {
        return status switch
        {
            DidStatus.Pending => NormalizedStatus.Pending,
            DidStatus.Active => NormalizedStatus.Active,
            DidStatus.PartnerFailure => NormalizedStatus.Error,
            DidStatus.CustomerValidationFailure => NormalizedStatus.InvalidData,
            _ => NormalizedStatus.Unknown
        };
    }
}
