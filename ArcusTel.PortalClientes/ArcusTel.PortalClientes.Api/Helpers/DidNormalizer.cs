using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Enum;
using ArcusTel.PortalClientes.Api.Interface;

namespace ArcusTel.PortalClientes.Api.Helpers;

public class DidNormalizer : IDidNormalizer
{
    public NormalizedActivationResponse FromPartnerBrasil(PartnerBrasilActivationResponse p)
    {
        return new NormalizedActivationResponse(
            p.Id.ToString(),                  // ExternalId
            p.DidNumber,                      // DidNumber
            PartnerId.BrasilConnect,          // PartnerId (enum from your Enum)
            InternalDidStatusMapper.FromInt(p.Status), // DidStatus mapped
            p.ErrorMessage,                   // DetailMessage
            p.CreatedAt                       // CreatedAt
        );
    }

    public NormalizedActivationResponse FromWorldTel(WorldTelActivationResponse w)
    {
        return new NormalizedActivationResponse(
            w.DidId,                          // ExternalId
            w.E164Number,                     // DidNumber
            PartnerId.WorldTel,               // PartnerId
            InternalDidStatusMapper.FromInt(w.Status), // DidStatus mapped
            null,                             // DetailMessage (WorldTel sample had none)
            w.CreatedAt                       // CreatedAt
        );
    }
}
