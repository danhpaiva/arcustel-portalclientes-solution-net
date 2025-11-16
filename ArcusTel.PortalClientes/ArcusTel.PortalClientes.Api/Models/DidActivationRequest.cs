using ArcusTel.PortalClientes.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcusTel.PortalClientes.Api.Models;

public class DidActivationRequest
{
    [Key]
    public long Id { get; set; }

    public string DidNumber { get; set; }
    public string PrefixCode { get; set; }
    public PartnerId PartnerId { get; set; }
    public DateTimeOffset RequestDate { get; set; }
    public DidStatus CurrentStatus { get; set; }
    public string LastPartnerRawStatus { get; set; }

    [ForeignKey("User")]
    public long UserId { get; set; }
}
