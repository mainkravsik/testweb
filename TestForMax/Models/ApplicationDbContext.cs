using Microsoft.EntityFrameworkCore;

namespace TestForMax.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ApiEndpoint> ApiEndpoints { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка внешнего ключа и каскадного удаления
        modelBuilder.Entity<ApiEndpoint>()
            .HasOne(a => a.Project)
            .WithMany(p => p.ApiEndpoints)
            .HasForeignKey(a => a.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 