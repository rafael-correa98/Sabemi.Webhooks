using Sabemi.Webhooks.Application.DTOs;
using Sabemi.Webhooks.Application.Interfaces;

namespace Sabemi.Webhooks.Application.Services;

public class PagamentoConsultaService : IPagamentoConsultaService
{
    private readonly IEventoBrutoRepository _eventoBrutoRepository;
    private readonly IStatusContratoRepository _statusContratoRepository;

    public PagamentoConsultaService(
        IEventoBrutoRepository eventoBrutoRepository,
        IStatusContratoRepository statusContratoRepository)
    {
        _eventoBrutoRepository = eventoBrutoRepository;
        _statusContratoRepository = statusContratoRepository;
    }

    public async Task<PaginacaoResponse<PagamentoConsultaResponse>> ListarPagamentosAsync(
        string? status, string? idContrato, int pagina, int tamanhoPagina, CancellationToken ct = default)
    {
        pagina = pagina < 1 ? 1 : pagina;
        tamanhoPagina = tamanhoPagina is < 1 or > 100 ? 20 : tamanhoPagina;

        var (itens, total) = await _eventoBrutoRepository.ListarAsync(status, idContrato, pagina, tamanhoPagina, ct);

        var itensResponse = itens.Select(e => new PagamentoConsultaResponse
        {
            Id = e.Id,
            IdTransacao = e.IdTransacao,
            IdContrato = e.IdContrato,
            Valor = e.Valor,
            DataPagamento = e.DataPagamento,
            StatusRecebido = e.StatusRecebido,
            Processado = e.Processado,
            ErroProcessamento = e.ErroProcessamento,
            RecebidoEm = e.RecebidoEm
        });

        return new PaginacaoResponse<PagamentoConsultaResponse>
        {
            Itens = itensResponse,
            Total = total,
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina
        };
    }

    public async Task<StatusContratoResponse?> ObterStatusContratoAsync(string idContrato, CancellationToken ct = default)
    {
        var status = await _statusContratoRepository.ObterPorIdContratoAsync(idContrato, ct);
        if (status is null) return null;

        return new StatusContratoResponse
        {
            IdContrato = status.IdContrato,
            UltimoIdTransacao = status.UltimoIdTransacao,
            StatusAtual = status.StatusAtual,
            ValorPago = status.ValorPago,
            DataUltimoPagamento = status.DataUltimoPagamento,
            AtualizadoEm = status.AtualizadoEm
        };
    }
}