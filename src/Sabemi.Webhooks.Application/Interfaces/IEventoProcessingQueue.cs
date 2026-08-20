using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Application.Interfaces;

public interface IEventoProcessingQueue
{
    ValueTask EnfileirarAsync(int eventoBrutoId, CancellationToken ct = default);
    IAsyncEnumerable<int> LerAsync(CancellationToken ct = default);
}