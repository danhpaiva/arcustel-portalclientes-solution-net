namespace ArcusTel.PortalClientes.Api.DTO;

public class WorldTelActivationResponse
{
    public int Id { get; set; }
    public string DidId { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTimeOffset? ActivationDate { get; set; }
    public string BillingCycle { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal MonthlyFee { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    public string E164Number => $"+{CountryCode}{AreaCode}{LocalNumber}";
}
