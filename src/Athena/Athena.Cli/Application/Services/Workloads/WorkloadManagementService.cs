using Athena.Cli.Application.Contracts.Workloads;
using Athena.Cli.Application.Interfaces.Workloads;

namespace Athena.Cli.Application.Services.Workloads;

public sealed class WorkloadManagementService(IWorkloadManagement management)
{
    private readonly IWorkloadManagement _management = management;
    public Task<IReadOnlyList<WorkloadLifecycleResponse>> GetAllAsync(CancellationToken token = default) => _management.GetAllAsync(token);
    public Task<WorkloadLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default) => _management.GetAsync(id, token);
    public Task<WorkloadLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default) => _management.CreateAsync(id, token);
    public Task<WorkloadLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) => _management.SetDesiredStateAsync(id, state, token);
    public Task<WorkloadOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default) => _management.ExecuteAsync(id, operation, token);
    public Task<IReadOnlyList<WorkloadOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default) => _management.GetOperationsAsync(id, token);
}
