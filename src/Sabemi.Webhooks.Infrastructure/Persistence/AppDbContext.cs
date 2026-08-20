using Microsoft.EntityFrameworkCore;
using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<EventoBruto> EventosBrutos => Set<EventoBruto>();
    public DbSet<StatusContrato> StatusContratos => Set<StatusContrato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}