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
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.LastClickedAt)
            .IsRequired(false);

        builder.HasIndex(x => x.ShortenedUrlId);
        builder.HasIndex(x => x.LastClickedAt);
    }
}