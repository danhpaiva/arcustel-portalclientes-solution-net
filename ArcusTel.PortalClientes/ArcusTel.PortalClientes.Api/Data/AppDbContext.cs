using Microsoft.EntityFrameworkCore;

namespace ArcusTel.PortalClientes.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ArcusTel.PortalClientes.Api.Models.DidActivationRequest> TB_DidActivationRequest { get; set; } = default!;
    public DbSet<ArcusTel.PortalClientes.Api.Models.PartnerResponseLog> TB_PartnerResponseLog { get; set; } = default!;
    public DbSet<ArcusTel.PortalClientes.Api.Models.PartnerStatusMapping> TB_PartnerStatusMapping { get; set; } = default!;
}
