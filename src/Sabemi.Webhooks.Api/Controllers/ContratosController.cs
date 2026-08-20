using Microsoft.AspNetCore.Mvc;
using Sabemi.Webhooks.Application.DTOs;
using Sabemi.Webhooks.Application.Interfaces;

namespace Sabemi.Webhooks.Api.Controllers;

[ApiController]
[Route("contratos")]
public class ContratosController : ControllerBase
{
    private readonly IPagamentoConsultaService _pagamentoConsultaService;

    public ContratosController(IPagamentoConsultaService pagamentoConsultaService)
    {
        _pagamentoConsultaService = pagamentoConsultaService;
    }

    [HttpGet("{id}/status")]
    [ProducesResponseType(typeof(StatusContratoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterStatus(string id, CancellationToken ct)
    {
        var status = await _pagamentoConsultaService.ObterStatusContratoAsync(id, ct);

        if (status is null)
            return NotFound(new { mensagem = $"Nenhum status encontrado para o contrato '{id}'" });

        return Ok(status);
    }
}