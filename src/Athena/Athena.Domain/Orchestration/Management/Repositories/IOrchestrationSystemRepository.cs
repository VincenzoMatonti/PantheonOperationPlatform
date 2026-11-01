using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Repositories;

public interface IOrchestrationSystemInstanceRepository
{
    Task<OrchestrationSystemInstance?> GetByIdAsync(OrchestrationSystemInstanceId id, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetByOrchestrationIdAsync(OrchestrationId orchestrationId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetBySystemInstanceIdAsync(SystemInstanceId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemInstance?> GetByOrchestrationAndSystemInstanceAsync(OrchestrationId orchestrationId, SystemInstanceId systemInstanceId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(OrchestrationSystemInstanceId id, CancellationToken cancellationToken = default);
    Task<bool> AssociationExistsAsync(OrchestrationId orchestrationId, SystemInstanceId systemId, bool includeDeleted = true, CancellationToken cancellationToken = default);
    Task AddAsync(OrchestrationSystemInstance association, CancellationToken cancellationToken = default);
    Task UpdateAsync(
    OrchestrationSystemInstance association,
    CancellationToken cancellationToken = default);
}