using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Areas.Identity.Data;

namespace MyLittleBusiness.Models
{
    public class MyLittleBusinessContext : IdentityDbContext<MyLittleBusinessUser>
    {
        public MyLittleBusinessContext(DbContextOptions<MyLittleBusinessContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<FactureHasItem> FactureHasItems { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
