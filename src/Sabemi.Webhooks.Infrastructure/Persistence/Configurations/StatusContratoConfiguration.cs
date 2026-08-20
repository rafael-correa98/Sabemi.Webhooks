using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Infrastructure.Persistence.Configurations;

public class StatusContratoConfiguration : IEntityTypeConfiguration<StatusContrato>
{
    public void Configure(EntityTypeBuilder<StatusContrato> builder)
    {
        builder.ToTable("StatusContratos");

        builder.HasKey(s => s.IdContrato);

        builder.Property(s => s.IdContrato)
            .HasMaxLength(100);

        builder.Property(s => s.UltimoIdTransacao)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.StatusAtual)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.ValorPago)
            .HasColumnType("decimal(18,2)");
    }
}