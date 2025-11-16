using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.Interface;

public interface IPartnerStatusHelper
{
    DidStatus Map(PartnerId partner, string partnerExternalStatus);
    DidStatus Map(PartnerId partner, int partnerStatusCode);
}
