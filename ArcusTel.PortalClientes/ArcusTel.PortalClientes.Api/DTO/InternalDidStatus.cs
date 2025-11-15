using ArcusTel.PortalClientes.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcusTel.PortalClientes.Api.DTO;

public record InternalDidStatus
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("DidActivationRequest")]
    public long RequestId { get; set; }

    public string DidNumber { get; set; }

    public DidStatus Status { get; set; }

    public string DetailMessage { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}
