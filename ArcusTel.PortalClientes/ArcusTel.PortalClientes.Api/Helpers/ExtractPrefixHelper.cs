using ArcusTel.PortalClientes.Api.Enum;
using System.Data;

namespace ArcusTel.PortalClientes.Api.Helpers;

public static class ExtractPrefixHelper
{
    /// <summary>
    /// Identifica se o DID é nacional (Brasil) ou internacional.
    /// </summary>
    public static DidType ExtractPrefix(string didNumber)
    {
        if (string.IsNullOrWhiteSpace(didNumber))
            return DidType.Unknown;

        string clean = didNumber.Replace("+", "");

        // Brasil → prefixo 55
        if (clean.StartsWith("55"))
            return DidType.National;

        return DidType.International;
    }
}
