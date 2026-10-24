using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Orchestrations;

public class AthenaOrchestrationManagementClient(AthenaApiClient apiClient) : IOrchestrationManagement
{
    private const string BasePath = "api/orchestrations/management";

    // ============================================================
    // LIFECYCLE COMMANDS
    public Task<OrchestrationLifecycleOperationResponse> ExecuteRuntimeOperationAsync(
        Guid orchestrationId, CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<CreateOrchestrationLifecycleOperationRequest, OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/{orchestrationId}/lifecycle/execute", request, cancellationToken);

    public Task<OrchestrationLifecycleResponse> CreateLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle", cancellationToken);

    public Task<OrchestrationLifecycleResponse> SetDesiredStateAsync(
        Guid orchestrationId, SetOrchestrationDesiredStateRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PatchAsync<SetOrchestrationDesiredStateRequest, OrchestrationLifecycleResponse>(
            $"{BasePath}/{orchestrationId}/lifecycle/desired-state", request, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkStartingAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle/starting", cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkRunningAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle/running", cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkStoppingAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle/stopping", cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkStoppedAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle/stopped", cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkFailedAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle/failed", cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkOperationalStatusUnknownAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle/unknown", cancellationToken);

    public Task<OrchestrationLifecycleResponse> DeleteLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle", cancellationToken);

    // ============================================================
    // LIFECYCLE QUERIES
    public Task<OrchestrationLifecycleResponse> GetLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<OrchestrationLifecycleResponse>($"{BasePath}/{orchestrationId}/lifecycle", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetAllLifecyclesAsync(
        CancellationToken cancellationToken = default)
        => apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleResponse>>(
            $"{BasePath}/lifecycles", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetLifecyclesByDesiredStateAsync(
        string desiredState, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleResponse>>(
            $"{BasePath}/lifecycles/by-desired-state?State={Encode(desiredState)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetLifecyclesByOperationalStatusAsync(
        string operationalStatus, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleResponse>>(
            $"{BasePath}/lifecycles/by-operational-status?Status={Encode(operationalStatus)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetDeletedLifecyclesAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleResponse>>($"{BasePath}/lifecycles/deleted", cancellationToken);

    // ============================================================
    // LIFECYCLE OPERATION COMMANDS
    public Task<OrchestrationLifecycleOperationResponse> CreateOperationAsync(
        Guid orchestrationId, CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<CreateOrchestrationLifecycleOperationRequest, OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/{orchestrationId}/lifecycle/operations", request, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> StartOperationAsync(
        Guid operationId,
        CancellationToken cancellationToken = default)
        => apiClient.PostAsync<OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/lifecycle-operations/{operationId}/start", cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> CompleteOperationAsync(
        Guid operationId,
        CancellationToken cancellationToken = default)
        => apiClient.PostAsync<OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/lifecycle-operations/{operationId}/complete", cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> FailOperationAsync(
        Guid operationId,
        FailOrchestrationLifecycleOperationRequest request,
        CancellationToken cancellationToken = default)
        => apiClient.PostAsync<FailOrchestrationLifecycleOperationRequest, OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/lifecycle-operations/{operationId}/fail",
            request, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> CancelOperationAsync(
        Guid operationId,
        CancellationToken cancellationToken = default)
        => apiClient.PostAsync<OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/lifecycle-operations/{operationId}/cancel", cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> DeleteOperationAsync(
        Guid operationId,
        CancellationToken cancellationToken = default)
        => apiClient.DeleteAsync<OrchestrationLifecycleOperationResponse>(
            $"{BasePath}/lifecycle-operations/{operationId}", cancellationToken);

    // ============================================================
    // LIFECYCLE OPERATION QUERIES
    public Task<OrchestrationLifecycleOperationResponse> GetOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<OrchestrationLifecycleOperationResponse>($"{BasePath}/lifecycle-operations/{operationId}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByOrchestrationIdAsync(
        Guid orchestrationId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/{orchestrationId}/lifecycle/operations", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetAllOperationsAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByTypeAsync(string type, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations/by-type?Type={Encode(type)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>(
            $"{BasePath}/lifecycle-operations/by-status?Status={Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByOrchestrationIdAndStatusAsync(
        Guid orchestrationId, string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>(
            $"{BasePath}/{orchestrationId}/lifecycle/operations/by-status?Status={Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetPendingOperationsAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations/pending", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetRunningOperationsAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations/running", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetFailedOperationsAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations/failed", cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationLifecycleOperationResponse>>($"{BasePath}/lifecycle-operations/deleted", cancellationToken);

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
