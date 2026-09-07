using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Repositories;

public interface IOperationRepository
{
    Task<Operation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Operation?> GetByExternalIdAsync(ExternalOperationId externalId, CancellationToken cancellationToken = default);

    Task<Operation?> GetByCorrelationIdAsync(CorrelationId correlationId, CancellationToken cancellationToken = default);

    Task<List<Operation?>> GetByStatusAsync(OperationStatus status, CancellationToken cancellationToken = default);

    Task<List<Operation?>> GetByRouteIdAsync(Guid routeId, CancellationToken cancellationToken = default);

    Task AddAsync(Operation operation, CancellationToken cancellationToken = default);

    void Update(Operation operation);
}