using Athena.Cli.Application.Contracts.Orchestrations;

namespace Athena.Cli.Application.Interfaces.Orchestrations;

public interface IOrchestrationManagement
{
    // Lifecycle commands
    Task<OrchestrationLifecycleResponse> CreateLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> SetDesiredStateAsync(Guid orchestrationId, SetOrchestrationDesiredStateRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> MarkStartingAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> MarkRunningAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> MarkStoppingAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> MarkStoppedAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> MarkFailedAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> MarkOperationalStatusUnknownAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleResponse> DeleteLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default);

    // Lifecycle queries
    Task<OrchestrationLifecycleResponse> GetLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetAllLifecyclesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetLifecyclesByDesiredStateAsync(string desiredState, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetLifecyclesByOperationalStatusAsync(string operationalStatus, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleResponse>> GetDeletedLifecyclesAsync(CancellationToken cancellationToken = default);

    // Lifecycle operation command
    Task<OrchestrationLifecycleOperationResponse> ExecuteRuntimeOperationAsync(Guid orchestrationId, CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationResponse> CreateOperationAsync(Guid orchestrationId, CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationResponse> StartOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationResponse> CompleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationResponse> FailOperationAsync(Guid operationId, FailOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationResponse> CancelOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationResponse> DeleteOperationAsync(Guid operationId, CancellationToken cancellationToken = default);

    // Lifecycle operation queries
    Task<OrchestrationLifecycleOperationResponse> GetOperationAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByOrchestrationIdAsync(Guid orchestrationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetAllOperationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetOperationsByOrchestrationIdAndStatusAsync(Guid orchestrationId, string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetPendingOperationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetRunningOperationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetFailedOperationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrchestrationLifecycleOperationResponse>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default);
}
