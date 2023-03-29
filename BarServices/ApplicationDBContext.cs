using BarServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BarServices
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(8,2);
        }
        public DbSet<Table> Tables => Set<Table>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Elaboration> Elaborations => Set<Elaboration>();

    }
}
