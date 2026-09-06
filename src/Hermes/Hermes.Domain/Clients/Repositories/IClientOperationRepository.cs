using Hermes.Domain.Clients.Entities;

namespace Hermes.Domain.Clients.Repositories;

public interface IClientOperationRepository
{
    Task<ClientOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ClientOperation?> GetAsync(Guid clientId, Guid operationTypeId, CancellationToken cancellationToken = default);

    Task<List<ClientOperation>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task AddAsync(ClientOperation clientOperation, CancellationToken cancellationToken = default);

    void Update(ClientOperation clientOperation);
}