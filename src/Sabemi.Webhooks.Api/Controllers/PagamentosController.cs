using Microsoft.AspNetCore.Mvc;
using Sabemi.Webhooks.Api.Filters;
using Sabemi.Webhooks.Application.DTOs;
using Sabemi.Webhooks.Application.Interfaces;

namespace Sabemi.Webhooks.Api.Controllers;

[ApiController]
[Route("webhooks")]
public class PagamentosController : ControllerBase
{
    private readonly IPagamentoWebhookService _pagamentoWebhookService;

    public PagamentosController(IPagamentoWebhookService pagamentoWebhookService)
    {
        _pagamentoWebhookService = pagamentoWebhookService;
    }

    [HttpPost("pagamento")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [ProducesResponseType(typeof(PagamentoWebhookResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ReceberPagamento(
        [FromBody] PagamentoWebhookRequest request,
        CancellationToken ct)
    {
        var resultado = await _pagamentoWebhookService.ReceberAsync(request, ct);

        // Responde rápido — o processamento pesado acontece em background (Fase 5)
        return Accepted(resultado);
    }
}