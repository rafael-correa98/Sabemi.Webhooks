using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Api.BackgroundServices;

public class PagamentoBackgroundWorker : BackgroundService
{
    private readonly IEventoProcessingQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PagamentoBackgroundWorker> _logger;

    public PagamentoBackgroundWorker(
        IEventoProcessingQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<PagamentoBackgroundWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var eventoBrutoId in _queue.LerAsync(stoppingToken))
        {
            try
            {
                await ProcessarEventoAsync(eventoBrutoId, stoppingToken);
            }
            catch (Exception ex)
            {
                // Nunca deixa uma exceção derrubar o worker — ele precisa continuar
                // processando os próximos eventos da fila mesmo se um falhar.
                _logger.LogError(ex, "Erro inesperado ao processar evento {EventoBrutoId}", eventoBrutoId);
            }
        }
    }

    private async Task ProcessarEventoAsync(int eventoBrutoId, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var eventoBrutoRepository = scope.ServiceProvider.GetRequiredService<IEventoBrutoRepository>();
        var statusContratoRepository = scope.ServiceProvider.GetRequiredService<IStatusContratoRepository>();

        var evento = await eventoBrutoRepository.ObterPorIdAsync(eventoBrutoId, ct);
        if (evento is null)
        {
            _logger.LogWarning("EventoBruto {EventoBrutoId} não encontrado para processamento", eventoBrutoId);
            return;
        }

        try
        {
            // Simula processamento pesado da regra de negócio (ex: chamada a sistema de seguros/empréstimos)
            await Task.Delay(2000, ct);

            var statusContrato = new StatusContrato(
                evento.IdContrato,
                evento.IdTransacao,
                evento.StatusRecebido,
                evento.Valor,
                evento.DataPagamento);

            await statusContratoRepository.UpsertAsync(statusContrato, ct);

            evento.MarcarComoProcessado();
            await eventoBrutoRepository.AtualizarAsync(evento, ct);

            _logger.LogInformation(
                "Evento {IdTransacao} processado com sucesso para o contrato {IdContrato}",
                evento.IdTransacao, evento.IdContrato);
        }
        catch (Exception ex)
        {
            evento.MarcarComoErro(ex.Message);
            await eventoBrutoRepository.AtualizarAsync(evento, ct);

            _logger.LogError(ex,
                "Falha ao processar evento {IdTransacao} do contrato {IdContrato}",
                evento.IdTransacao, evento.IdContrato);
        }
    }
}