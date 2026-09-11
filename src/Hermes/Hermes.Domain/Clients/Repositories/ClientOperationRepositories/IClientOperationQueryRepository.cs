using Hermes.Domain.Clients.Entities;

namespace Hermes.Domain.Clients.Repositories;

public interface IClientOperationQueryRepository
{
    Task<ClientOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ClientOperation?> GetAsync(Guid clientId, Guid operationTypeId, CancellationToken cancellationToken = default);

    Task<List<ClientOperation>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
}