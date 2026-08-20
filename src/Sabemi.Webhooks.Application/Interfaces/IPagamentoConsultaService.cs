using Sabemi.Webhooks.Application.DTOs;

namespace Sabemi.Webhooks.Application.Interfaces;

public interface IPagamentoConsultaService
{
    Task<PaginacaoResponse<PagamentoConsultaResponse>> ListarPagamentosAsync(
        string? status, string? idContrato, int pagina, int tamanhoPagina, CancellationToken ct = default);

    Task<StatusContratoResponse?> ObterStatusContratoAsync(string idContrato, CancellationToken ct = default);
}