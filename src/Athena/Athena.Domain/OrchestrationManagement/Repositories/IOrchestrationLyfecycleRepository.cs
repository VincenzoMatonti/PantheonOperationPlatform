using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Enums;
using Athena.Domain.OrchestrationRegistry.ValueObjects;

namespace Athena.Domain.OrchestrationManagement.Repositories;

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
