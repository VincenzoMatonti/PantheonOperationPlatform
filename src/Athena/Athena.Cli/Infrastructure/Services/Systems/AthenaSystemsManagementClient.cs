using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Systems;

public sealed class AthenaSystemsManagementClient(AthenaApiClient apiClient) : ISystemsManagement
{
    private const string BasePath = "api/systems/management";

    // Lifecycle queries
    public Task<SystemLifecycleResponse> GetLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<SystemLifecycleResponse>($"{BasePath}/{systemId}/lifecycle", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetAllLifecyclesAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleResponse>>($"{BasePath}/lifecycles", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByDesiredStateAsync(string desiredState, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleResponse>>($"{BasePath}/lifecycles/by-desired-state?DesiredState={Encode(desiredState)}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByOperationalStatusAsync(string operationalStatus, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleResponse>>(
        $"{BasePath}/lifecycles/by-operational-status?OperationalStatus={Encode(operationalStatus)}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetLifecyclesByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleResponse>>($"{BasePath}/lifecycles/by-deletion-status?IsDeleted={isDeleted}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleResponse>> GetDeletedLifecyclesAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleResponse>>($"{BasePath}/lifecycles/deleted", cancellationToken);

    // Lifecycle commands
    public Task<SystemLifecycleResponse> CreateLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemLifecycleResponse>($"{BasePath}/{systemId}/lifecycle", cancellationToken);

    public Task<SystemLifecycleResponse> SetDesiredStateAsync(Guid systemId, SetSystemDesiredStateRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PatchAsync<SetSystemDesiredStateRequest, SystemLifecycleResponse>($"{BasePath}/{systemId}/lifecycle/desired-state", request, cancellationToken);

    public Task<SystemLifecycleResponse> MarkStartingAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        PostLifecycleTransitionAsync(systemId, "starting", cancellationToken);

    public Task<SystemLifecycleResponse> MarkRunningAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        PostLifecycleTransitionAsync(systemId, "running", cancellationToken);

    public Task<SystemLifecycleResponse> MarkStoppingAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        PostLifecycleTransitionAsync(systemId, "stopping", cancellationToken);

    public Task<SystemLifecycleResponse> MarkStoppedAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        PostLifecycleTransitionAsync(systemId, "stopped", cancellationToken);

    public Task<SystemLifecycleResponse> MarkFailedAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        PostLifecycleTransitionAsync(systemId, "failed", cancellationToken);

    public Task<SystemLifecycleResponse> MarkUnknownAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        PostLifecycleTransitionAsync(systemId, "unknown", cancellationToken);

    public Task DeleteLifecycleAsync(Guid systemId, CancellationToken cancellationToken = default) => DeleteLifecycleAsyncCore(systemId, cancellationToken);

    // Lifecycle operation queries
    public Task<SystemLifecycleOperationResponse> GetOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<SystemLifecycleOperationResponse>($"{BasePath}/lifecycle-operations/{operationId}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleOperationResponse>>($"{BasePath}/{systemId}/lifecycle/operations", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleOperationResponse>>(
        $"{BasePath}/lifecycle-operations/by-status?Status={Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsBySystemIdAndStatusAsync(
        Guid systemId, string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleOperationResponse>>(
        $"{BasePath}/{systemId}/lifecycle/operations/by-status?Status={Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetOperationsByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleOperationResponse>>(
        $"{BasePath}/lifecycle-operations/by-deletion-status?IsDeleted={isDeleted}", cancellationToken);

    public Task<IReadOnlyList<SystemLifecycleOperationResponse>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations/deleted", cancellationToken);

    // Lifecycle operation commands
    public Task<SystemLifecycleOperationResponse> CreateOperationAsync(
        Guid systemId, CreateSystemLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<CreateSystemLifecycleOperationRequest, SystemLifecycleOperationResponse>(
        $"{BasePath}/{systemId}/lifecycle/operations", request, cancellationToken);

    public Task<SystemLifecycleOperationResponse> StartOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        PostOperationActionAsync(operationId, "start", cancellationToken);

    public Task<SystemLifecycleOperationResponse> CompleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        PostOperationActionAsync(operationId, "complete", cancellationToken);

    public Task<SystemLifecycleOperationResponse> FailOperationAsync(
        Guid operationId, FailSystemLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<FailSystemLifecycleOperationRequest, SystemLifecycleOperationResponse>(
            $"{BasePath}/lifecycle-operations/{operationId}/fail", request, cancellationToken);

    public Task<SystemLifecycleOperationResponse> CancelOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        PostOperationActionAsync(operationId, "cancel", cancellationToken);

    public Task DeleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default) => DeleteOperationAsyncCore(operationId, cancellationToken);

    private Task<SystemLifecycleResponse> PostLifecycleTransitionAsync(Guid systemId, string transition, CancellationToken cancellationToken) =>
        apiClient.PostAsync<SystemLifecycleResponse>($"{BasePath}/{systemId}/lifecycle/{transition}", cancellationToken);

    private Task<SystemLifecycleOperationResponse> PostOperationActionAsync(Guid operationId, string action, CancellationToken cancellationToken) =>
        apiClient.PostAsync<SystemLifecycleOperationResponse>($"{BasePath}/lifecycle-operations/{operationId}/{action}", cancellationToken);


    private async Task DeleteLifecycleAsyncCore(Guid systemId, CancellationToken cancellationToken)
    {
        _ = await apiClient.DeleteAsync<SystemLifecycleResponse>($"{BasePath}/{systemId}/lifecycle", cancellationToken);
    }

    private async Task DeleteOperationAsyncCore(Guid operationId, CancellationToken cancellationToken)
    {
        _ = await apiClient.DeleteAsync<SystemLifecycleOperationResponse>($"{BasePath}/lifecycle-operations/{operationId}", cancellationToken);
    }

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
