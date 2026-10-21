using Athena.Cli.Application.Contracts.Orchestrations;

namespace Athena.Cli.Application.Interfaces.Orchestrations;

public interface IOrchestrationsRegistry
{
    // Commands
    Task<OrchestrationResponse> CreateAsync(CreateOrchestrationRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> RegisterAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> UnregisterAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> EnableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> DisableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> RenameAsync(Guid id, RenameOrchestrationRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    // Queries
    Task<OrchestrationResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrchestrationResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationResponse>> GetByRegistrationStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationResponse>> GetByAvailabilityStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationResponse>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
