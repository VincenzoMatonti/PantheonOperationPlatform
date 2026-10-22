using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.ValueObjects;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.OrchestrationManagement.Repositories;

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
    void Update(OrchestrationSystem association);
}