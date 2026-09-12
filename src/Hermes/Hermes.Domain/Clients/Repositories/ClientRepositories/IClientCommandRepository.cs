using Hermes.Domain.Clients.Entities;

namespace Hermes.Domain.Clients.Repositories.ClientRepositories;

public interface IClientCommandRepository
{    
    Task AddAsync(Client client, CancellationToken cancellationToken = default);
    void Update(Client client);
}