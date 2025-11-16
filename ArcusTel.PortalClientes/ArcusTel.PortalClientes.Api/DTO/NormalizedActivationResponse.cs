using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.DTO;

public record NormalizedActivationResponse(
    string ExternalId,      // id criado pelo parceiro (ex: "WT-INT-...")
    string DidNumber,       // +5511...
    PartnerId PartnerId,    // enum (PartnerBrasil, WorldTel)
    DidStatus Status,       // enum interno (Pending, Active, Failed, Cancelled, etc.)
    string? DetailMessage,  // mensagem human-readable
    DateTimeOffset CreatedAt
);

