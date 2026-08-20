using Sabemi.Webhooks.Application.DTOs;
using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Domain.Entities;
using Sabemi.Webhooks.Domain.Exceptions;

namespace Sabemi.Webhooks.Application.Services;

public class PagamentoWebhookService : IPagamentoWebhookService
{
    private readonly IEventoBrutoRepository _eventoBrutoRepository;
    private readonly IEventoProcessingQueue _queue;

    public PagamentoWebhookService(
        IEventoBrutoRepository eventoBrutoRepository,
        IEventoProcessingQueue queue)
    {
        _eventoBrutoRepository = eventoBrutoRepository;
        _queue = queue;
    }

    public async Task<PagamentoWebhookResponse> ReceberAsync(PagamentoWebhookRequest request, CancellationToken ct = default)
    {
        var jaExiste = await _eventoBrutoRepository.ExisteTransacaoAsync(request.IdTransacao, ct);
        if (jaExiste)
        {
            return PagamentoWebhookResponse.JaProcessado();
        }

        var payloadJson = System.Text.Json.JsonSerializer.Serialize(request);
        var evento = new EventoBruto(
            request.IdTransacao,
            request.IdContrato,
            request.Valor,
            request.DataPagamento,
            request.Status,
            payloadJson);

        try
        {
            await _eventoBrutoRepository.AdicionarAsync(evento, ct);
        }
        catch (TransacaoDuplicadaException)
        {
            // Condição de corrida: dois requests quase simultâneos com o mesmo IdTransacao.
            // A constraint UNIQUE do banco pegou o que a checagem em memória não pegou.
            return PagamentoWebhookResponse.JaProcessado();
        }

        await _queue.EnfileirarAsync(evento.Id, ct);

        return PagamentoWebhookResponse.Aceitar();
    }
}