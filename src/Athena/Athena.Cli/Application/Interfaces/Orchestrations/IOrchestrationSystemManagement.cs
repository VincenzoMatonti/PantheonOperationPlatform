using Athena.Cli.Application.Contracts.Orchestrations;

namespace Athena.Cli.Application.Interfaces.Orchestrations;

public interface IOrchestrationSystemInstanceManagement
{
    Task<OrchestrationSystemInstanceResponse> CreateAsync(CreateOrchestrationSystemInstanceRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemInstanceResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemInstanceResponse> RestoreAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationSystemInstanceResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationSystemInstanceResponse>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
}
