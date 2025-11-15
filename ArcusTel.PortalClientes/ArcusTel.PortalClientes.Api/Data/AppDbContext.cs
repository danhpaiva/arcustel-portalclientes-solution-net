using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArcusTel.PortalClientes.Api.Models;

namespace ArcusTel.PortalClientes.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArcusTel.PortalClientes.Api.Models.DidActivationRequest> DidActivationRequest { get; set; } = default!;
        public DbSet<ArcusTel.PortalClientes.Api.Models.PartnerResponseLog> PartnerResponseLog { get; set; } = default!;
        public DbSet<ArcusTel.PortalClientes.Api.Models.PartnerStatusMapping> PartnerStatusMapping { get; set; } = default!;
    }
}
