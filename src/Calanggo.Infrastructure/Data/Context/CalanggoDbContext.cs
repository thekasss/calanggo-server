using Microsoft.EntityFrameworkCore;
using Calanggo.Domain.Entities;
using Calanggo.Infrastructure.Data.EntitiyConfiguration;

namespace Calanggo.Infrastructure.Data.Context;

public class CalangoDbContext(DbContextOptions<CalangoDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public DbSet<UrlStatistics> UrlStatistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        // OR:

        modelBuilder.ApplyConfiguration(new ShortenedUrlConfiguration());
        modelBuilder.ApplyConfiguration(new UrlStatisticsConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}