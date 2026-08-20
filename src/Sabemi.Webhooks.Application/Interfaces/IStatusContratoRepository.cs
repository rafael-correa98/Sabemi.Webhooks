using Sabemi.Webhooks.Domain.Entities;

namespace Sabemi.Webhooks.Application.Interfaces;

public interface IStatusContratoRepository
{
    Task<StatusContrato?> ObterPorIdContratoAsync(string idContrato, CancellationToken ct = default);
    Task UpsertAsync(StatusContrato statusContrato, CancellationToken ct = default);
}