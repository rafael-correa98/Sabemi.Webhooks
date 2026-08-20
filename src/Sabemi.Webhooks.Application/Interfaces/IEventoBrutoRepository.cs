using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Application.Interfaces;

public interface IEventoBrutoRepository
{
    Task<bool> ExisteTransacaoAsync(string idTransacao, CancellationToken ct = default);
    Task AdicionarAsync(EventoBruto evento, CancellationToken ct = default);
    Task AtualizarAsync(EventoBruto evento, CancellationToken ct = default);
    Task<EventoBruto?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<(IEnumerable<EventoBruto> Itens, int Total)> ListarAsync(
        string? status, string? idContrato, int pagina, int tamanhoPagina, CancellationToken ct = default);
}