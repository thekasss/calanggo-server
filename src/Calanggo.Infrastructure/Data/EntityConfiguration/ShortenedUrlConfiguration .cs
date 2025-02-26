using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Calanggo.Domain.Entities;

namespace Calanggo.Infrastructure.Data.EntityConfiguration;

public class ShortenedUrlConfiguration : IEntityTypeConfiguration<ShortenedUrl>
{
    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.ToTable("ShortenedUrls");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OriginalUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(x => x.ShortCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(256);
                
        // relacionamentos
        builder.HasOne(x => x.Statistics)
            .WithOne(x => x.ShortenedUrl)
            .HasForeignKey<UrlStatistics>(x => x.ShortenedUrlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}