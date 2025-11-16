namespace ArcusTel.PortalClientes.Api.Enum;

public enum DidStatus
{
    // 0 - Valor Padrão/Inicial
    Pending = 0,

    // Ativação em Curso/Espera de Ação
    InProgress = 1,

    // Requer Validação de Documentos/Dados do Cliente
    WaitingValidation = 2,

    // Ativação Concluída com Sucesso
    Active = 3,

    // Falha que pode ser do Parceiro (API Down, Erro 5xx, etc.)
    PartnerFailure = 4,

    // Falha de Negócio/Validação do Cliente (ex: Documento Inválido)
    CustomerValidationFailure = 5,

    UnknownFailure = 6
}
