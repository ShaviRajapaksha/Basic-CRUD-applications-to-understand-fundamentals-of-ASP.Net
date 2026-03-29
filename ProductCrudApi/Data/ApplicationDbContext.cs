using Microsoft.EntityFrameworkCore;
using ProductCrudApi.Models;

namespace ProductCrudApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasPrecision(18, 2);
        }
    }
}