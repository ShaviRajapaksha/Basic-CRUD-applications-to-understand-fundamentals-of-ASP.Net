using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure one-to-many relationship between Post and Comment
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure indexes for better performance
        modelBuilder.Entity<Post>()
            .HasIndex(p => p.CreatedAt);

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.PostId);
    }
}