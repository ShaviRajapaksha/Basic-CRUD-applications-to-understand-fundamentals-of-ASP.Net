using Microsoft.EntityFrameworkCore;

namespace ProductCrudApiV2.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext>options)
        : base(options)
    {   
    }
    public DbSet<ProductCrudApiV2.Models.Product> Products { get; set; }
}