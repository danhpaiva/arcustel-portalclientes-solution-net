using ArcusTel.PortalClientes.Api.Interface;

namespace ArcusTel.PortalClientes.Api.Helpers;

public class PrefixHelper : IPrefixHelper
{
    public bool IsBrazilian(string e164)
    {
        if (string.IsNullOrWhiteSpace(e164)) return false;
        var v = e164.StartsWith("+") ? e164.Substring(1) : e164;
        return v.StartsWith("55");
    }

    public string ExtractPrefix(string e164)
    {
        if (string.IsNullOrWhiteSpace(e164)) return string.Empty;
        if (IsBrazilian(e164))
        {
            // +55 + DDD at least: return +55 + ddd => e.g. +5511
            return e164.Length >= 4 ? e164.Substring(0, 4) : e164;
        }
        // international: return first 5 as fallback
        return e164.Length >= 5 ? e164.Substring(0, 5) : e164;
    }
}

