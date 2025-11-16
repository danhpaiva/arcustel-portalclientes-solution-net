namespace ArcusTel.PortalClientes.Api.DTO;

public class WorldTelAuthResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
