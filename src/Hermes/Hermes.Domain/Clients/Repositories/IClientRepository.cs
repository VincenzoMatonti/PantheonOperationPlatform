using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;

namespace Hermes.Domain.Clients.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Client?> GetByCodeAsync(ClientCode code, CancellationToken cancellationToken = default);

    Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Client client, CancellationToken cancellationToken = default);

    void Update(Client client);
}