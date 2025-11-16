using ArcusTel.PortalClientes.Api.DTO;

namespace ArcusTel.PortalClientes.Api.Interface;

public interface IPartnerBrasilClient
{
    /// <summary>
    /// Envia pedido de ativação ao PartnerBrasil e retorna a resposta bruta desserializada.
    /// </summary>
    Task<PartnerBrasilActivationResponse?> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default);

    /// <summary>
    /// Consulta status por número (se aplicável)
    /// </summary>
    Task<PartnerBrasilActivationResponse?> GetStatusByNumberAsync(string e164Number, CancellationToken ct = default);
}
