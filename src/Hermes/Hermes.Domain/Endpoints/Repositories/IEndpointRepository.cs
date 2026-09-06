using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.ValueObjects;

namespace Hermes.Domain.Endpoints.Repositories;

public interface IEndpointRepository
{
    Task<Endpoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Endpoint?> GetByCodeAsync(EndpointCode code, CancellationToken cancellationToken = default);

    Task<Endpoint?> GetByTypeAsync(EndpointType type, CancellationToken cancellationToken = default);

    Task<List<Endpoint>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Endpoint endpoint, CancellationToken cancellationToken = default);

    void Update(Endpoint endpoint);
}