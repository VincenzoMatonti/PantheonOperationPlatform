using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;

namespace Athena.Cli.Application.Services.Orchestrations;

public class OrchestrationSystemManagementService(IOrchestrationSystemManagement management)
{
    private readonly IOrchestrationSystemManagement _management = management;

    public Task<OrchestrationSystemResponse> CreateAsync(CreateOrchestrationSystemRequest request, CancellationToken cancellationToken = default) =>
        _management.CreateAsync(request, cancellationToken);

    public Task<OrchestrationSystemResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        _management.DeleteAsync(id, cancellationToken);

    public Task<OrchestrationSystemResponse> RestoreAsync(Guid id, CancellationToken cancellationToken = default) =>
        _management.RestoreAsync(id, cancellationToken);

    public Task<OrchestrationSystemResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _management.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<OrchestrationSystemResponse>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default) =>
        _management.GetAllAsync(includeDeleted, cancellationToken);
}
