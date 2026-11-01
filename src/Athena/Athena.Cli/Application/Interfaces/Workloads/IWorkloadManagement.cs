using Athena.Cli.Application.Contracts.Workloads;

namespace Athena.Cli.Application.Interfaces.Workloads;

public interface IWorkloadManagement
{
    Task<IReadOnlyList<WorkloadLifecycleResponse>> GetAllAsync(CancellationToken token = default);
    Task<WorkloadLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default);
    Task<WorkloadLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default);
    Task<WorkloadLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default);
    Task<WorkloadOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default);
    Task<IReadOnlyList<WorkloadOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default);
}
