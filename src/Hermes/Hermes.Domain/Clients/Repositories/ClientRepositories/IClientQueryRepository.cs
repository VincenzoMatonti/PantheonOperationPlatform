using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;

namespace Hermes.Domain.Clients.Repositories.ClientRepositories;

public interface IClientQueryRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Client?> GetByCodeAsync(ClientCode code, CancellationToken cancellationToken = default);
    Task<List<ClientCode>> GetAllClientCodeAsync(CancellationToken cancellationToken = default);
    Task<ClientCode?> GetClientCodeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Client>> GetActiveClientAsync(CancellationToken cancellationToken = default);
    Task<bool> IsActiveClientAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Client>> GetNonActiveClientAsync(CancellationToken cancellationToken = default);
    Task<bool> IsNonActiveClientAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Client>> GetDeletedClientAsync(CancellationToken cancellationToken = default);
    Task<bool> IsDeletedClientAsync(Guid id, CancellationToken cancellationToken = default);
}