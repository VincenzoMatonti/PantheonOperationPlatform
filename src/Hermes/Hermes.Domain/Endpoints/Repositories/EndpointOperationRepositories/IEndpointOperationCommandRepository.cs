using Hermes.Domain.Endpoints.Entities;

namespace Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;

public interface IEndpointOperationCommandRepository
{
    Task AddAsync(EndpointOperation endpointOperation, CancellationToken cancellationToken = default);
    void Update(EndpointOperation endpointOperation);
}