using Hermes.Domain.Routes.Entities;

namespace Hermes.Domain.Routes.Repositories;

public interface IRouteRepository
{
    // Single parameter
    Task<List<Route>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);

    // Two parameters
    Task<List<Route>> GetByClientIdAndOperationTypeIdAsync(Guid clientId, Guid operationTypeId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetByClientIdAndEndpointIdAsync(Guid clientId, Guid endpointId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetByOperationTypeIdAndEndpointIdAsync(Guid operationTypeId, Guid endpointId, CancellationToken cancellationToken = default);

    // Three parameters
    Task<Route?> GetAsync(Guid clientId, Guid operationTypeId, Guid endpointId, CancellationToken cancellationToken = default);

    // Route identifier
    Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Global
    Task<List<Route>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Route>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<List<Route>> GetNonActiveAsync(CancellationToken cancellationToken = default);
    Task<List<Route>> GetDeletedAsync(CancellationToken cancellationToken = default);

    // By ClientId
    Task<List<Route>> GetActiveByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetNonActiveByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetDeletedByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    // By OperationTypeId
    Task<List<Route>> GetActiveByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetNonActiveByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetDeletedByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default);

    // By EndpointId
    Task<List<Route>> GetActiveByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetNonActiveByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<List<Route>> GetDeletedByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(Route route, CancellationToken cancellationToken = default);
    void Update(Route route);
}

