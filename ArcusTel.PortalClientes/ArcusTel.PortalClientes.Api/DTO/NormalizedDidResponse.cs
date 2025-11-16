using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.DTO;

public class NormalizedDidResponse
{
    public string DidNumber { get; set; }
    public string Partner { get; set; }
    public NormalizedStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}

