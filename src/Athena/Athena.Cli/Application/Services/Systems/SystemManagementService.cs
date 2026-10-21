using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;

namespace Athena.Cli.Application.Services.Systems;

public class SystemManagementService(ISystemsManagement systemsManagement)
{
    // Lifecycle queries
    public Task<SystemLifecycleResponse> GetLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.GetLifecycleAsync(systemId, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetAllLifecyclesAsync(CancellationToken cancellationToken = default) =>
        systemsManagement.GetAllLifecyclesAsync(cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByDesiredStateAsync(string desiredState, CancellationToken cancellationToken = default) =>
        systemsManagement.GetLifecyclesByDesiredStateAsync(desiredState, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByOperationalStatusAsync(string operationalStatus, CancellationToken cancellationToken = default) =>
        systemsManagement.GetLifecyclesByOperationalStatusAsync(operationalStatus, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
        systemsManagement.GetLifecyclesByDeletionStatusAsync(isDeleted, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetDeletedLifecyclesAsync(CancellationToken cancellationToken = default) =>
        systemsManagement.GetDeletedLifecyclesAsync(cancellationToken);

    // Lifecycle commands
    public Task<SystemLifecycleResponse> CreateLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.CreateLifecycleAsync(systemId, cancellationToken);

    public Task<SystemLifecycleResponse> SetDesiredStateAsync(Guid systemId, SetSystemDesiredStateRequest request, CancellationToken cancellationToken = default) =>
        systemsManagement.SetDesiredStateAsync(systemId, request, cancellationToken);

    public Task<SystemLifecycleResponse> MarkStartingAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.MarkStartingAsync(systemId, cancellationToken);

    public Task<SystemLifecycleResponse> MarkRunningAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.MarkRunningAsync(systemId, cancellationToken);

    public Task<SystemLifecycleResponse> MarkStoppingAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.MarkStoppingAsync(systemId, cancellationToken);

    public Task<SystemLifecycleResponse> MarkStoppedAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.MarkStoppedAsync(systemId, cancellationToken);

    public Task<SystemLifecycleResponse> MarkFailedAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.MarkFailedAsync(systemId, cancellationToken);

    public Task<SystemLifecycleResponse> MarkUnknownAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.MarkUnknownAsync(systemId, cancellationToken);

    public Task DeleteLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.DeleteLifecycleAsync(systemId, cancellationToken);

    // Lifecycle operation queries
    public Task<SystemLifecycleOperationResponse> GetOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        systemsManagement.GetOperationAsync(operationId, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        systemsManagement.GetOperationsBySystemIdAsync(systemId, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken = default) =>
        systemsManagement.GetOperationsByStatusAsync(status, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsBySystemIdAndStatusAsync(
        Guid systemId, string status, CancellationToken cancellationToken = default) =>
        systemsManagement.GetOperationsBySystemIdAndStatusAsync(systemId, status, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
        systemsManagement.GetOperationsByDeletionStatusAsync(isDeleted, cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default) =>
        systemsManagement.GetDeletedOperationsAsync(cancellationToken);

    // Lifecycle operation commands
    public Task<SystemLifecycleOperationResponse> CreateOperationAsync(
        Guid systemId, CreateSystemLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        systemsManagement.CreateOperationAsync(systemId, request, cancellationToken);

    public Task<SystemLifecycleOperationResponse> StartOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        systemsManagement.StartOperationAsync(operationId, cancellationToken);

    public Task<SystemLifecycleOperationResponse> CompleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        systemsManagement.CompleteOperationAsync(operationId, cancellationToken);

    public Task<SystemLifecycleOperationResponse> FailOperationAsync(
        Guid operationId, FailSystemLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        systemsManagement.FailOperationAsync(operationId, request, cancellationToken);

    public Task<SystemLifecycleOperationResponse> CancelOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        systemsManagement.CancelOperationAsync(operationId, cancellationToken);

    public Task DeleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        systemsManagement.DeleteOperationAsync(operationId, cancellationToken);
}
