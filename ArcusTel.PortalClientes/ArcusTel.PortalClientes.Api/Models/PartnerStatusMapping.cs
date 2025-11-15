using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.Models;

public class PartnerStatusMapping
{
    public long Id { get; set; }
    public PartnerId PartnerId { get; set; }
    public string PartnerExternalStatus { get; set; }
    public DidStatus InternalNormalizedStatus { get; set; }
    public string Description { get; set; }
}
