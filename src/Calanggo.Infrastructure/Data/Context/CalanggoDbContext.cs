using Microsoft.EntityFrameworkCore;
using Calanggo.Domain.Entities;
using Calanggo.Infrastructure.Data.EntitiyConfiguration;

namespace Calanggo.Infrastructure.Data.Context;

public class CalanggoDbContext(DbContextOptions<CalanggoDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public DbSet<UrlStatistics> UrlStatistics { get; set; }
    public DbSet<DeviceMetric> DeviceMetrics { get; set; }
    public DbSet<LocationMetric> LocationMetrics { get; set; }
    public DbSet<ClickEvent> ClickEvents { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        // OR:

        // modelBuilder.ApplyConfiguration(new ShortenedUrlConfiguration());
        // modelBuilder.ApplyConfiguration(new UrlStatisticsConfiguration());
        // modelBuilder.ApplyConfiguration(new DeviceMetricConfiguration());
        // modelBuilder.ApplyConfiguration(new LocationMetricConfiguration());
        // modelBuilder.ApplyConfiguration(new ClickEventConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}