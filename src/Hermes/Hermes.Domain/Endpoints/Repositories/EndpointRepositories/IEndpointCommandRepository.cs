using Hermes.Domain.Endpoints.Entities;

namespace Hermes.Domain.Endpoints.Repositories.EndpointRepositories;

public interface IEndpointCommandRepository
{
    Task AddAsync(Endpoint endpoint, CancellationToken cancellationToken = default);
    void Update(Endpoint endpoint);
}