using System.ComponentModel.DataAnnotations;

namespace ArcusTel.PortalClientes.Api.Models;

public class User
{
    [Key]
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
}
