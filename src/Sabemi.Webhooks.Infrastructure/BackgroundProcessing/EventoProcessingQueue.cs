using System.Threading.Channels;
using Sabemi.Webhooks.Application.Interfaces;

namespace Sabemi.Webhooks.Infrastructure.BackgroundProcessing;

public class EventoProcessingQueue : IEventoProcessingQueue
{
    private readonly Channel<int> _channel;

    public EventoProcessingQueue()
    {
        // Unbounded: não bloqueia o request do webhook esperando espaço na fila.
        // Para um cenário de produção com alto volume, valeria considerar um canal limitado (bounded)
        // com estratégia de backpressure — fora do escopo deste teste técnico.
        _channel = Channel.CreateUnbounded<int>();
    }

    public async ValueTask EnfileirarAsync(int eventoBrutoId, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(eventoBrutoId, ct);
    }

    public IAsyncEnumerable<int> LerAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}