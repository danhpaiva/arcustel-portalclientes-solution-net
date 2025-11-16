namespace ArcusTel.PortalClientes.Api.DTO;

public class WorldTelDidResponse
{
    public int Id { get; set; }
    public string DidId { get; set; }
    public string CountryCode { get; set; }
    public string AreaCode { get; set; }
    public string LocalNumber { get; set; }
    public int Status { get; set; }
    public DateTime? ActivationDate { get; set; }
    public string BillingCycle { get; set; }
    public string Currency { get; set; }
    public decimal MonthlyFee { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Campo necessário para o normalized
    public string E164Number => $"+{CountryCode}{AreaCode}{LocalNumber}";
}

