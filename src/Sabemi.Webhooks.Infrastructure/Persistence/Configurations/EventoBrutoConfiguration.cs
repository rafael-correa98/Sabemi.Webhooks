using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Infrastructure.Persistence.Configurations;

public class EventoBrutoConfiguration : IEntityTypeConfiguration<EventoBruto>
{
    public void Configure(EntityTypeBuilder<EventoBruto> builder)
    {
        builder.ToTable("EventosBrutos");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdTransacao)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.IdTransacao)
            .IsUnique(); // garante idempotência no nível do banco

        builder.Property(e => e.IdContrato)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Valor)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.StatusRecebido)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Payload)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.ErroProcessamento)
            .HasMaxLength(1000);
    }
}