using ArcusTel.PortalClientes.Api.DTO;

namespace ArcusTel.PortalClientes.Api.Interface;

public interface IDidOrchestrator
{
    Task<NormalizedActivationResponse> ActivateAsync(string e164Number, long userId, CancellationToken ct = default);
    Task<NormalizedActivationResponse> GetStatusAsync(long requestId, CancellationToken ct = default);
}
