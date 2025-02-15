using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Calanggo.Domain.Entities;

namespace Calanggo.Infrastructure.Data.EntitiyConfiguration;

public class UrlStatisticsConfiguration : IEntityTypeConfiguration<UrlStatistics>
{
    public void Configure(EntityTypeBuilder<UrlStatistics> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TotalClicks)
            .IsRequired();
            
        builder.Property(x => x.LastClickedAt)
            .IsRequired(false);
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // indices
        builder.HasIndex(x => x.ShortenedUrlId);
        builder.HasIndex(x => x.LastClickedAt);

        // relacionamentos
        builder.HasOne(x => x.ShortenedUrl)
            .WithOne(x => x.Statistics)
            .HasForeignKey<UrlStatistics>(x => x.ShortenedUrlId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ClickEvents)
            .WithOne(x => x.UrlStatistics)
            .HasForeignKey(x => x.UrlStatisticsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.LocationMetrics)
            .WithOne(x => x.UrlStatistics)
            .HasForeignKey(x => x.UrlStatisticsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DeviceMetrics)
            .WithOne(x => x.UrlStatistics)
            .HasForeignKey(x => x.UrlStatisticsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}