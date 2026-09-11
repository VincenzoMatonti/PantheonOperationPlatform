using Hermes.Domain.Clients.Entities;

namespace Hermes.Domain.Clients.Repositories.ClientRepositories;

public interface IClientCommandRepository
{
    Task<bool> IsActiveClientAsync(Client client, CancellationToken cancellationToken = default);

    Task <bool> IsNonActiveClientAsync(Client client, CancellationToken cancellationToken = default);

    Task <bool> IsDeletedClientAsync(Client client, CancellationToken cancellationToken = default);

    Task AddAsync(Client client, CancellationToken cancellationToken = default);

    void Update(Client client);
}