using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.ValueObjects;

namespace Hermes.Domain.Endpoints.Repositories.EndpointRepositories;

public interface IEndpointRepository
{
    Task<Endpoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Endpoint?> GetByCodeAsync(EndpointCode code, CancellationToken cancellationToken = default);
    Task<Endpoint?> GetByTypeAsync(EndpointType type, CancellationToken cancellationToken = default);
    Task<List<Endpoint>> GetAllAsync(CancellationToken cancellationToken = default);  
     Task<List<EndpointCode>> GetAllEndpointCodeAsync(CancellationToken cancellationToken = default);
    Task<EndpointCode?> GetEndpointCodeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Endpoint>> GetActiveEndpointAsync(CancellationToken cancellationToken = default);
    Task<List<Endpoint>> GetNonActiveEndpointAsync(CancellationToken cancellationToken = default);
    Task<List<Endpoint>> GetDeletedEndpointAsync(CancellationToken cancellationToken = default);
}