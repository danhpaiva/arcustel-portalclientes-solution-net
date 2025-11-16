namespace ArcusTel.PortalClientes.Api.DTO;

public class WorldTelActivationResponse
{
    public long Id { get; set; }
    public string DidId { get; set; } // didId = "WT-INT-..."
    public string CountryCode { get; set; }
    public string AreaCode { get; set; }
    public string LocalNumber { get; set; }
    public int Status { get; set; }
    public DateTimeOffset? ActivationDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
