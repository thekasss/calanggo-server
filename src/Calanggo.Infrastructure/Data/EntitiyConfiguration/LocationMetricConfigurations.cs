using Calanggo.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calanggo.Infrastructure.Data.EntitiyConfiguration;

public class LocationMetricConfiguration : IEntityTypeConfiguration<LocationMetric>
{
    public void Configure(EntityTypeBuilder<LocationMetric> builder)
    {
        builder.HasKey(x => x.Id);

        // Propriedades
        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Region)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Clicks)
            .IsRequired()
            .HasDefaultValue(0);

        // // indices
        // builder.HasIndex(x => x.UrlStatisticsId);

        // // indice composto
        // builder.HasIndex(x => new { x.Country, x.Region, x.City });
        
        // // indice unico para evitar duplicatas
        // builder.HasIndex(x => new { x.UrlStatisticsId, x.Country, x.Region, x.City })
        //     .IsUnique();
    }
}