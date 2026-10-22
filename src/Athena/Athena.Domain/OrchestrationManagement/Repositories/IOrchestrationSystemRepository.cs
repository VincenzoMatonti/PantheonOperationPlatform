using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.ValueObjects;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.OrchestrationManagement.Repositories;

public interface IOrchestrationSystemRepository
{
    Task<OrchestrationSystem?> GetByIdAsync(OrchestrationSystemId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationSystem>> GetByOrchestrationIdAsync(OrchestrationId orchestrationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationSystem>> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(OrchestrationId orchestrationId, SystemId systemId, CancellationToken cancellationToken = default);

    Task AddAsync(OrchestrationSystem association, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrchestrationSystem association, CancellationToken cancellationToken = default);
}