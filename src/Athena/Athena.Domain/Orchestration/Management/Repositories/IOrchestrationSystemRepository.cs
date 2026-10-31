using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Repositories;

public interface IOrchestrationSystemRepository
{
    Task<OrchestrationSystem?> GetByIdAsync(OrchestrationSystemId id, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrchestrationSystem>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrchestrationSystem>> GetByOrchestrationIdAsync(OrchestrationId orchestrationId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrchestrationSystem>> GetBySystemIdAsync(SystemId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<OrchestrationSystem?> GetByOrchestrationAndSystemAsync(OrchestrationId orchestrationId, SystemId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(OrchestrationSystemId id, CancellationToken cancellationToken = default);
    Task<bool> AssociationExistsAsync(OrchestrationId orchestrationId, SystemId systemId, bool includeDeleted = true, CancellationToken cancellationToken = default);
    Task AddAsync(OrchestrationSystem association, CancellationToken cancellationToken = default);
    Task UpdateAsync(
    OrchestrationSystem association,
    CancellationToken cancellationToken = default);
}