using Microsoft.EntityFrameworkCore;
using Pingu.Core.Domain.Entities;
using Pingu.Infrastructure.Data.EntitiyConfiguration;

namespace Pingu.Infrastructure.Data.Context;

public class PinguDbContext(DbContextOptions<PinguDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public DbSet<UrlStatistics> UrlStatistics { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        // OR:

        modelBuilder.ApplyConfiguration(new ShortenedUrlConfiguration());
        modelBuilder.ApplyConfiguration(new UrlStatisticsConfiguration());
    }
}