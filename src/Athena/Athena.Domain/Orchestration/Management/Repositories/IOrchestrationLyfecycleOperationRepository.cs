using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Enums;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Repositories;

public interface IOrchestrationLifecycleOperationRepository
{
    Task<OrchestrationLifecycleOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetByOrchestrationIdAsync(OrchestrationId orchestrationId, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetByTypeAsync(OrchestrationLifecycleOperationType type, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetByStatusAsync(OrchestrationLifecycleOperationStatus status, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetByOrchestrationIdAndStatusAsync(OrchestrationId orchestrationId, OrchestrationLifecycleOperationStatus status, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetRunningAsync(CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetFailedAsync(CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycleOperation>> GetDeletedAsync(CancellationToken cancellationToken = default);
    Task AddAsync(OrchestrationLifecycleOperation operation, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrchestrationLifecycleOperation operation, CancellationToken cancellationToken = default);
}