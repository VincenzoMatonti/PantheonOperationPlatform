using Athena.Cli.Application.Contracts.Systems;

namespace Athena.Cli.Application.Interfaces.Systems;

public interface ISystemsManagement
{
    // Lifecycle queries
    Task<SystemLifecycleResponse> GetLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleResponse>> GetAllLifecyclesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByDesiredStateAsync(string desiredState, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByOperationalStatusAsync(string operationalStatus, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleResponse>> GetDeletedLifecyclesAsync(CancellationToken cancellationToken = default);

    // Lifecycle commands
    Task<SystemLifecycleResponse> CreateLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> SetDesiredStateAsync(Guid systemId, SetSystemDesiredStateRequest request, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> MarkStartingAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> MarkRunningAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> MarkStoppingAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> MarkStoppedAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> MarkFailedAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleResponse> MarkUnknownAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task DeleteLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default);

    // Lifecycle operation queries
    Task<SystemLifecycleOperationResponse> GetOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsBySystemIdAndStatusAsync(Guid systemId, string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default);
    
    // Lifecycle operation commands
    Task<SystemLifecycleOperationResponse> CreateOperationAsync(Guid systemId, CreateSystemLifecycleOperationRequest request, CancellationToken cancellationToken = default);
    Task<SystemLifecycleOperationResponse> StartOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleOperationResponse> CompleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<SystemLifecycleOperationResponse> FailOperationAsync(Guid operationId, FailSystemLifecycleOperationRequest request, CancellationToken cancellationToken = default);
    Task<SystemLifecycleOperationResponse> CancelOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task DeleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
}
