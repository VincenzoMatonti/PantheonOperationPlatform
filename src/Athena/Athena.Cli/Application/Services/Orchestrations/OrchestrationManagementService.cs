using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;

namespace Athena.Cli.Application.Services.Orchestrations;

public class OrchestrationManagementService(IOrchestrationManagement management)
{
    private readonly IOrchestrationManagement _management = management;

    // ============================================================
    // LIFECYCLE COMMANDS
    public Task<OrchestrationLifecycleResponse> CreateLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.CreateLifecycleAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> SetDesiredStateAsync(
        Guid orchestrationId, SetOrchestrationDesiredStateRequest request, CancellationToken cancellationToken = default) =>
        _management.SetDesiredStateAsync(orchestrationId, request, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkStartingAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.MarkStartingAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkRunningAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.MarkRunningAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkStoppingAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.MarkStoppingAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkStoppedAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.MarkStoppedAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkFailedAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.MarkFailedAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> MarkOperationalStatusUnknownAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.MarkOperationalStatusUnknownAsync(orchestrationId, cancellationToken);

    public Task<OrchestrationLifecycleResponse> DeleteLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.DeleteLifecycleAsync(orchestrationId, cancellationToken);

    // ============================================================
    // LIFECYCLE QUERIES
    public Task<OrchestrationLifecycleResponse> GetLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.GetLifecycleAsync(orchestrationId, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetAllLifecyclesAsync(CancellationToken cancellationToken = default) =>
        _management.GetAllLifecyclesAsync(cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetLifecyclesByDesiredStateAsync(string desiredState, CancellationToken cancellationToken = default) =>
        _management.GetLifecyclesByDesiredStateAsync(desiredState, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetLifecyclesByOperationalStatusAsync(
        string operationalStatus, CancellationToken cancellationToken = default) =>
        _management.GetLifecyclesByOperationalStatusAsync(operationalStatus, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetDeletedLifecyclesAsync(CancellationToken cancellationToken = default) =>
        _management.GetDeletedLifecyclesAsync(cancellationToken);

    // ============================================================
    // LIFECYCLE OPERATION COMMANDS
    public Task<OrchestrationLifecycleOperationResponse> CreateOperationAsync(
        Guid orchestrationId, CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        _management.CreateOperationAsync(orchestrationId, request, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> StartOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        _management.StartOperationAsync(operationId, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> CompleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        _management.CompleteOperationAsync(operationId, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> FailOperationAsync(
        Guid operationId, FailOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default) =>
        _management.FailOperationAsync(operationId, request, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> CancelOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        _management.CancelOperationAsync(operationId, cancellationToken);

    public Task<OrchestrationLifecycleOperationResponse> DeleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        _management.DeleteOperationAsync(operationId, cancellationToken);

    // ============================================================
    // LIFECYCLE OPERATION QUERIES
    public Task<OrchestrationLifecycleOperationResponse> GetOperationAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        _management.GetOperationAsync(operationId, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByOrchestrationIdAsync(
        Guid orchestrationId, CancellationToken cancellationToken = default) =>
        _management.GetOperationsByOrchestrationIdAsync(orchestrationId, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetAllOperationsAsync(CancellationToken cancellationToken = default) =>
        _management.GetAllOperationsAsync(cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByTypeAsync(
        string type, CancellationToken cancellationToken = default) =>
        _management.GetOperationsByTypeAsync(type, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByStatusAsync(
        string status, CancellationToken cancellationToken = default) =>
        _management.GetOperationsByStatusAsync(status, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByOrchestrationIdAndStatusAsync(
        Guid orchestrationId, string status, CancellationToken cancellationToken = default) =>
        _management.GetOperationsByOrchestrationIdAndStatusAsync(orchestrationId, status, cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetPendingOperationsAsync(CancellationToken cancellationToken = default) =>
        _management.GetPendingOperationsAsync(cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetRunningOperationsAsync(CancellationToken cancellationToken = default) =>
        _management.GetRunningOperationsAsync(cancellationToken);

    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetFailedOperationsAsync(CancellationToken cancellationToken = default) =>
        _management.GetFailedOperationsAsync(cancellationToken);
    public Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default) =>
        _management.GetDeletedOperationsAsync(cancellationToken);
}
