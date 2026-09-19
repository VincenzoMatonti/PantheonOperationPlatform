using Hermes.Domain.Endpoints.Entities;

namespace Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;

public interface IEndpointOperationQueryRepository
{
    Task<EndpointOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EndpointOperation?> GetAsync(Guid endpointId, Guid operationTypeId, CancellationToken cancellationToken = default);
    Task<List<EndpointOperation>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);
    Task<List<EndpointOperation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<EndpointOperation>> GetActiveEndpointOperation(CancellationToken cancellationToken = default);
    Task<List<EndpointOperation>> GetNonActiveEndpointOperation(CancellationToken cancellationToken = default);
    Task<List<EndpointOperation>> GetDeletedEndpointOperation(CancellationToken cancellationToken = default);
}