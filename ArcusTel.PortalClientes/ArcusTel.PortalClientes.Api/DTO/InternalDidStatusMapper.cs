using ArcusTel.PortalClientes.Api.Enum;

namespace ArcusTel.PortalClientes.Api.DTO;

public static class InternalDidStatusMapper
{
    /// <summary>
    /// Mapeia códigos inteiros do parceiro para o enum interno DidStatus
    /// Ajuste se precisar de mapeamentos específicos por parceiro.
    /// </summary>
    public static DidStatus FromInt(int code) => code switch
    {
        0 => DidStatus.Pending,
        1 => DidStatus.Active,
        2 => DidStatus.PartnerFailure,
        3 => DidStatus.CustomerValidationFailure,
        _ => DidStatus.UnknownFailure
    };
}
