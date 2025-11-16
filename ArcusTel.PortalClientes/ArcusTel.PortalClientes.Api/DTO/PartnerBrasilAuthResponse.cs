using System.Text.Json.Serialization;

namespace ArcusTel.PortalClientes.Api.DTO;

public class PartnerBrasilAuthResponse
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}