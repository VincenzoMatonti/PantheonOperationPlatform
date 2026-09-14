using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Repositories.OperationRepositories;

public interface IOperationQueryRepository
{
    Task<List<Operation>> GetAll(CancellationToken cancellationToken = default);
    Task<List<Operation>> GetAllDeleted(CancellationToken cancellationToken = default);
    Task<List<OperationTypeCode>> GetAllCode(CancellationToken cancellationToken = default);
    Task<Operation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Operation?> GetByCode (OperationTypeCode code, CancellationToken cancellationToken = default); 
    Task<Operation?> GetByExternalIdAsync(ExternalOperationId externalId, CancellationToken cancellationToken = default);
    Task<Operation?> GetByCorrelationIdAsync(CorrelationId correlationId, CancellationToken cancellationToken = default);
    Task<List<Operation>> GetByStatusAsync(OperationStatus status, CancellationToken cancellationToken = default);
    Task<List<Operation>> GetByExecutionIdAsync(Guid executionId, CancellationToken cancellationToken = default);
}