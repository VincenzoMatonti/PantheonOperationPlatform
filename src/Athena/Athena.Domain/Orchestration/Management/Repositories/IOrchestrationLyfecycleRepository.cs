using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Enums;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Repositories;

public interface IOrchestrationLifecycleRepository
{
    Task<OrchestrationLifecycle?> GetByIdAsync(OrchestrationId id, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycle>> GetByDesiredStateAsync(OrchestrationDesiredState state, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycle>> GetByOperationalStatusAsync(OrchestrationOperationalStatus status, CancellationToken cancellationToken = default);
    Task<List<OrchestrationLifecycle>> GetDeletedAsync(CancellationToken cancellationToken = default);
    Task AddAsync(OrchestrationLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrchestrationLifecycle lifecycle, CancellationToken cancellationToken = default);
}
