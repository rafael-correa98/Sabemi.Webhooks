using Microsoft.EntityFrameworkCore;
using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Domain.Entities;
using Sabemi.Webhooks.Infrastructure.Persistence;

namespace Sabemi.Webhooks.Infrastructure.Repositories;

public class StatusContratoRepository : IStatusContratoRepository
{
    private readonly AppDbContext _context;

    public StatusContratoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StatusContrato?> ObterPorIdContratoAsync(string idContrato, CancellationToken ct = default)
    {
        return await _context.StatusContratos
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.IdContrato == idContrato, ct);
    }

    public async Task UpsertAsync(StatusContrato statusContrato, CancellationToken ct = default)
    {
        var existente = await _context.StatusContratos
            .FirstOrDefaultAsync(s => s.IdContrato == statusContrato.IdContrato, ct);

        if (existente is null)
        {
            _context.StatusContratos.Add(statusContrato);
        }
        else
        {
            existente.AtualizarStatus(
                statusContrato.UltimoIdTransacao,
                statusContrato.StatusAtual,
                statusContrato.ValorPago,
                statusContrato.DataUltimoPagamento);
        }

        await _context.SaveChangesAsync(ct);
    }
}