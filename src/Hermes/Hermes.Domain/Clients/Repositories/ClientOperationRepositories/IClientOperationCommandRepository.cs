using Hermes.Domain.Clients.Entities;

namespace Hermes.Domain.Clients.Repositories;

public interface IClientOperationCommandRepository
{
    Task AddAsync(ClientOperation clientOperation, CancellationToken cancellationToken = default);

    void Update(ClientOperation clientOperation);
}