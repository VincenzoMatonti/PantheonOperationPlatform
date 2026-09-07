using Hermes.Domain.Endpoints.Entities;

namespace Hermes.Domain.Endpoints.Repositories;

public interface IEndpointOperationRepository
{
    Task<EndpointOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<EndpointOperation?> GetAsync(Guid endpointId, Guid operationTypeId, CancellationToken cancellationToken = default);

    Task<List<EndpointOperation>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default);

    Task AddAsync(EndpointOperation endpointOperation, CancellationToken cancellationToken = default);

    void Update(EndpointOperation endpointOperation);
}