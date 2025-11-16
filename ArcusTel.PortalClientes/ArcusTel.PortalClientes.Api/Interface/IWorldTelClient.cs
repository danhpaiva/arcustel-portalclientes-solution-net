using ArcusTel.PortalClientes.Api.DTO;

namespace ArcusTel.PortalClientes.Api.Interface;

public interface IWorldTelClient
{
    Task<WorldTelActivationResponse?> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default);
    Task<WorldTelActivationResponse?> GetStatusByDidIdAsync(string didId, CancellationToken ct = default);
}
