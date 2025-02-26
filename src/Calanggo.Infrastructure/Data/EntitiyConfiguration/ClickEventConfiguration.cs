using Calanggo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calanggo.Infrastructure.Data.EntitiyConfiguration;

public class ClickEventConfiguration : IEntityTypeConfiguration<ClickEvent>
{
    public void Configure(EntityTypeBuilder<ClickEvent> builder)
    {
        builder.HasKey(x => x.Id);

        // propriedades obrigatórias
        builder.Property(x => x.ClickedAt)
            .IsRequired();

        // ipv6 pode ter até 45 caracteres
        builder.Property(x => x.IpAddress)
            .IsRequired()
            .HasMaxLength(45);

        builder.Property(x => x.UserAgent)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Referer)
            .HasMaxLength(2048);

        // propriedades de localização
        builder.Property(x => x.Country)
            .HasMaxLength(99);

        builder.Property(x => x.Region)
            .HasMaxLength(99);

        builder.Property(x => x.City)
            .HasMaxLength(99);

        // propriedades do dispositivo
        builder.Property(x => x.DeviceType)
            .HasMaxLength(50);

        builder.Property(x => x.Browser)
            .HasMaxLength(50);

        builder.Property(x => x.OperatingSystem)
            .HasMaxLength(50);

        // // indices
        // builder.HasIndex(x => x.UrlStatisticsId);
        // builder.HasIndex(x => x.ClickedAt);
        // builder.HasIndex(x => x.Country);
        // builder.HasIndex(x => x.DeviceType);
    }
}