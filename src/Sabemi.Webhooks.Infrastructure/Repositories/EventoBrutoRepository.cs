using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Domain.Entities;
using Sabemi.Webhooks.Domain.Exceptions;
using Sabemi.Webhooks.Infrastructure.Persistence;

namespace Sabemi.Webhooks.Infrastructure.Repositories;

public class EventoBrutoRepository : IEventoBrutoRepository
{
    private readonly AppDbContext _context;

    public EventoBrutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteTransacaoAsync(string idTransacao, CancellationToken ct = default)
    {
        return await _context.EventosBrutos
            .AsNoTracking()
            .AnyAsync(e => e.IdTransacao == idTransacao, ct);
    }

    public async Task AdicionarAsync(EventoBruto evento, CancellationToken ct = default)
    {
        try
        {
            _context.EventosBrutos.Add(evento);
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (EhViolacaoDeIndiceUnico(ex))
        {
            throw new TransacaoDuplicadaException(evento.IdTransacao);
        }
    }

    public async Task AtualizarAsync(EventoBruto evento, CancellationToken ct = default)
    {
        _context.EventosBrutos.Update(evento);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<EventoBruto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.EventosBrutos
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<(IEnumerable<EventoBruto> Itens, int Total)> ListarAsync(
        string? status, string? idContrato, int pagina, int tamanhoPagina, CancellationToken ct = default)
    {
        var query = _context.EventosBrutos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.StatusRecebido == status);

        if (!string.IsNullOrWhiteSpace(idContrato))
            query = query.Where(e => e.IdContrato == idContrato);

        var total = await query.CountAsync(ct);

        var itens = await query
            .OrderByDescending(e => e.RecebidoEm)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(ct);

        return (itens, total);
    }

    private static bool EhViolacaoDeIndiceUnico(DbUpdateException ex)
    {
        // SQL Server: 2601 = índice único duplicado, 2627 = violação de constraint única/PK
        return ex.InnerException is SqlException sqlEx
            && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
    }
}