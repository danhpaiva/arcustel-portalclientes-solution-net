namespace ArcusTel.PortalClientes.Api.Interface;

public interface IPartnerClient
{
    Task AuthenticateAsync(CancellationToken ct = default);
    Task<(string rawResponse, object parsed)> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default);
    Task<(string rawResponse, object parsed)> GetStatusAsync(string externalIdOrNumber, CancellationToken ct = default);
}

