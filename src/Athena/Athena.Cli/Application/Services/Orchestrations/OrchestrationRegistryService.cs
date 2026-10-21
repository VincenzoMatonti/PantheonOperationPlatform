using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;

namespace Athena.Cli.Application.Services.Orchestrations;

public class OrchestrationRegistryService(IOrchestrationsRegistry registry)
{
    private readonly IOrchestrationsRegistry _registry = registry;

    // Commands
    public Task<OrchestrationResponse> CreateAsync(CreateOrchestrationRequest request, CancellationToken cancellationToken = default) =>
        _registry.CreateAsync(request, cancellationToken);

    public Task<OrchestrationResponse> RegisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.RegisterAsync(id, cancellationToken);

    public Task<OrchestrationResponse> UnregisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.UnregisterAsync(id, cancellationToken);

    public Task<OrchestrationResponse> EnableAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.EnableAsync(id, cancellationToken);

    public Task<OrchestrationResponse> DisableAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.DisableAsync(id, cancellationToken);

    public Task<OrchestrationResponse> RenameAsync(Guid id, RenameOrchestrationRequest request, CancellationToken cancellationToken = default) =>
        _registry.RenameAsync(id, request, cancellationToken);

    public Task<OrchestrationResponse> MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.MarkAvailableAsync(id, cancellationToken);

    public Task<OrchestrationResponse> MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.MarkUnavailableAsync(id, cancellationToken);

    public Task<OrchestrationResponse> MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.MarkAvailabilityUnknownAsync(id, cancellationToken);

    public Task<OrchestrationResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.DeleteAsync(id, cancellationToken);

    // Queries
    public Task<OrchestrationResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _registry.GetByIdAsync(id, cancellationToken);

    public Task<OrchestrationResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        _registry.GetByNameAsync(name, cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _registry.GetAllAsync(cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetByRegistrationStatusAsync(string status, CancellationToken cancellationToken = default) =>
        _registry.GetByRegistrationStatusAsync(status, cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetByAvailabilityStatusAsync(string status, CancellationToken cancellationToken = default) =>
        _registry.GetByAvailabilityStatusAsync(status, cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetDeletedAsync(CancellationToken cancellationToken = default) =>
        _registry.GetDeletedAsync(cancellationToken);
}
