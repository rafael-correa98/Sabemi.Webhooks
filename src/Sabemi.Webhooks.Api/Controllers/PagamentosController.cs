using Microsoft.AspNetCore.Mvc;
using Sabemi.Webhooks.Api.Filters;
using Sabemi.Webhooks.Application.DTOs;
using Sabemi.Webhooks.Application.Interfaces;

namespace Sabemi.Webhooks.Api.Controllers;

[ApiController]
public class PagamentosController : ControllerBase
{
    private readonly IPagamentoWebhookService _pagamentoWebhookService;
    private readonly IPagamentoConsultaService _pagamentoConsultaService;

    public PagamentosController(
        IPagamentoWebhookService pagamentoWebhookService,
        IPagamentoConsultaService pagamentoConsultaService)
    {
        _pagamentoWebhookService = pagamentoWebhookService;
        _pagamentoConsultaService = pagamentoConsultaService;
    }

    [HttpPost("webhooks/pagamento")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [ProducesResponseType(typeof(PagamentoWebhookResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ReceberPagamento(
        [FromBody] PagamentoWebhookRequest request,
        CancellationToken ct)
    {
        var resultado = await _pagamentoWebhookService.ReceberAsync(request, ct);
        return Accepted(resultado);
    }

    [HttpGet("pagamentos")]
    [ProducesResponseType(typeof(PaginacaoResponse<PagamentoConsultaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPagamentos(
        [FromQuery] string? status,
        [FromQuery] string? idContrato,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 20,
        CancellationToken ct = default)
    {
        var resultado = await _pagamentoConsultaService.ListarPagamentosAsync(status, idContrato, pagina, tamanhoPagina, ct);
        return Ok(resultado);
    }
}