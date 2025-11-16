namespace ArcusTel.PortalClientes.Api.DTO;

public class PartnerBrasilActivationResponse
{
    public int Id { get; set; }
    public string DidNumber { get; set; } = string.Empty;
    public int Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
