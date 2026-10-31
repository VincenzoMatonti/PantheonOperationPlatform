using Athena.Domain.Orchestration.Registry.Enums;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using OrchestrationEntity = Athena.Domain.Orchestration.Registry.Entities.Orchestration;

namespace Athena.Domain.Orchestration.Registry.Repositories;

public interface IOrchestrationRepository
{
    Task<OrchestrationEntity?> GetByIdAsync(OrchestrationId id, CancellationToken cancellationToken = default);
    Task<OrchestrationEntity?> GetByNameAsync(OrchestrationName name, CancellationToken cancellationToken = default);
    Task<List<OrchestrationEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<OrchestrationEntity>> GetByRegistrationStatusAsync(OrchestrationRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<List<OrchestrationEntity>> GetByAvailabilityStatusAsync(OrchestrationAvailabilityStatus status, CancellationToken cancellationToken = default);
    Task<List<OrchestrationEntity>> GetDeletedAsync(CancellationToken cancellationToken = default);
    Task AddAsync(OrchestrationEntity orchestration, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrchestrationEntity orchestration, CancellationToken cancellationToken = default);
}
