using ArcusTel.PortalClientes.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcusTel.PortalClientes.Api.Models;

public class PartnerStatusMapping
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public PartnerId PartnerId { get; set; }
    public string PartnerExternalStatus { get; set; }
    public DidStatus InternalNormalizedStatus { get; set; }
    public string Description { get; set; }
}
