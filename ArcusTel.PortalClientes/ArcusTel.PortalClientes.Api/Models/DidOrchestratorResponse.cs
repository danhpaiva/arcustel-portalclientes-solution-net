using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcusTel.PortalClientes.Api.Models;

public class DidOrchestratorResponse
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }

    // DID retornado já normalizado pelo orchestrator
    public string? Did { get; set; }

    // Status final calculado (por ex: Active, Inactive, Pending)
    public string? Status { get; set; }

    // (Opcional) Identificação do parceiro escolhido
    public string? PartnerSource { get; set; }

    // (Opcional) Dados brutos retornados do parceiro
    public object? RawPartnerResponse { get; set; }
}
