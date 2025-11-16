namespace ArcusTel.PortalClientes.Api.DTO;

public class PartnerBrasilDidResponse
{
    public int Id { get; set; }
    public string DidNumber { get; set; }
    public int Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

