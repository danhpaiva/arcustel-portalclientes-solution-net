using ArcusTel.PortalClientes.Api.DTO;

namespace ArcusTel.PortalClientes.Api.Interface;

public interface IDidNormalizer
{
    NormalizedActivationResponse FromPartnerBrasil(PartnerBrasilActivationResponse p);
    NormalizedActivationResponse FromWorldTel(WorldTelActivationResponse w);
}
