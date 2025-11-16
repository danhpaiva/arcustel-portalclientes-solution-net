namespace ArcusTel.PortalClientes.Api.DTO;

public class PartnerBrasilActivationResponse
{
    public long Id { get; set; }
    public string DidNumber { get; set; }
    public int Status { get; set; } // partner specific code
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
