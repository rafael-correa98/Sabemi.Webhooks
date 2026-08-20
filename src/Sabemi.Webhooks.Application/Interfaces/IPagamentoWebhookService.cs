using Sabemi.Webhooks.Application.DTOs;

namespace Sabemi.Webhooks.Application.Interfaces;

public interface IPagamentoWebhookService
{
    Task<PagamentoWebhookResponse> ReceberAsync(PagamentoWebhookRequest request, CancellationToken ct = default);
}