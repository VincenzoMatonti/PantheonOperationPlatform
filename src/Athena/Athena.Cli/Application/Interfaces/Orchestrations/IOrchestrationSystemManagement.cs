using Athena.Cli.Application.Contracts.Orchestrations;

namespace Athena.Cli.Application.Interfaces.Orchestrations;

public interface IOrchestrationSystemManagement
{
    Task<OrchestrationSystemResponse> CreateAsync(CreateOrchestrationSystemRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemResponse> RestoreAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationSystemResponse>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
}
