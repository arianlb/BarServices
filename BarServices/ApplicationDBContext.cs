using BarServices.Models;
using Microsoft.EntityFrameworkCore;

namespace BarServices
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(8,2);
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Table> Tables => Set<Table>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Elaboration> Elaborations => Set<Elaboration>();

    }
}
