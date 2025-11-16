namespace ArcusTel.PortalClientes.Api.Interface;

public interface IPrefixHelper
{
    bool IsBrazilian(string e164);
    string ExtractPrefix(string e164);
}
