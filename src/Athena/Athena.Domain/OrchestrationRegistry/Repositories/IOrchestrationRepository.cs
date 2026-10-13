
using Athena.Domain.OrchestrationRegistry.Entities;
using Athena.Domain.OrchestrationRegistry.Enums;
using Athena.Domain.OrchestrationRegistry.ValueObjects;

namespace Athena.Domain.OrchestrationRegistry.Repositories;

public interface IOrchestrationRepository
{
    Task<Orchestration?> GetByIdAsync(OrchestrationId id, CancellationToken cancellationToken = default);
    Task<Orchestration?> GetByNameAsync(OrchestrationName name, CancellationToken cancellationToken = default);
    Task<List<Orchestration>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Orchestration>> GetByRegistrationStatusAsync(OrchestrationRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<List<Orchestration>> GetByAvailabilityStatusAsync(OrchestrationAvailabilityStatus status, CancellationToken cancellationToken = default);
    Task<List<Orchestration>> GetDeletedAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Orchestration orchestration, CancellationToken cancellationToken = default);
    Task UpdateAsync(Orchestration orchestration, CancellationToken cancellationToken = default);
}
