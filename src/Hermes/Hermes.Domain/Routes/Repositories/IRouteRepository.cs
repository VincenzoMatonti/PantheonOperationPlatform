using Hermes.Domain.Routes.Entities;

namespace Hermes.Domain.Routes.Repositories;

public interface IRouteRepository
{
    Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Route?> GetAsync(Guid clientId, Guid operationTypeId, Guid endpointId, CancellationToken cancellationToken = default);

    Task<List<Route>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<List<Route>> GetByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default);

    Task<List<Route>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);

    Task AddAsync(Route route, CancellationToken cancellationToken = default);

    void Update(Route route);
}