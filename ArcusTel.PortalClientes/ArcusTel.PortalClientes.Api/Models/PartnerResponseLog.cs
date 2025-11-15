using ArcusTel.PortalClientes.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcusTel.PortalClientes.Api.Models;

public class PartnerResponseLog
{
    [Key]
    public long LogId { get; set; }

    [ForeignKey("DidActivationRequest")]
    public long RequestId { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    // A resposta JSON completa do parceiro (objeto, array, aninhado, etc.)
    public string RawResponsePayload { get; set; }

    public string PartnerStatus { get; set; }

    public DidStatus NormalizerOutputStatus { get; set; }

    public string PartnerDetailMessage { get; set; }
}
