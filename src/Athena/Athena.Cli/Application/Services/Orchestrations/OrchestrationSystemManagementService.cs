using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;

namespace Athena.Cli.Application.Services.Orchestrations;

public class OrchestrationSystemInstanceManagementService(IOrchestrationSystemInstanceManagement management)
{
    private readonly IOrchestrationSystemInstanceManagement _management = management;

    public Task<OrchestrationSystemInstanceResponse> CreateAsync(CreateOrchestrationSystemInstanceRequest request, CancellationToken cancellationToken = default) =>
        _management.CreateAsync(request, cancellationToken);

    public Task<OrchestrationSystemInstanceResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        _management.DeleteAsync(id, cancellationToken);

    public Task<OrchestrationSystemInstanceResponse> RestoreAsync(Guid id, CancellationToken cancellationToken = default) =>
        _management.RestoreAsync(id, cancellationToken);

    public Task<OrchestrationSystemInstanceResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _management.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<OrchestrationSystemInstanceResponse>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default) =>
        _management.GetAllAsync(includeDeleted, cancellationToken);
}
