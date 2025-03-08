using Calanggo.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calanggo.Infrastructure.Data.EntitiyConfiguration;

public class DeviceMetricConfiguration : IEntityTypeConfiguration<DeviceMetric>
{
    public void Configure(EntityTypeBuilder<DeviceMetric> builder)
    {
        builder.ToTable("DeviceMetrics");
        builder.HasKey(x => x.Id);

        // propriedades
        builder.Property(x => x.DeviceType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Browser)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Clicks)
            .IsRequired()
            .HasDefaultValue(0);

        // indices
        builder.HasIndex(x => x.UrlStatisticsId);
        builder.HasIndex(x => new { x.DeviceType, x.Browser });

        // indice unico para evitar duplicatas
        builder.HasIndex(x => new { x.UrlStatisticsId, x.DeviceType, x.Browser })
            .IsUnique();
    }
}