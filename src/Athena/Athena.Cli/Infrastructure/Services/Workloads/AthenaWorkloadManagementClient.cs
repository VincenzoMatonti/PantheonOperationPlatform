using Athena.Cli.Application.Contracts.Workloads;
using Athena.Cli.Application.Interfaces.Workloads;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Workloads;

public sealed class AthenaWorkloadManagementClient(AthenaApiClient client) : IWorkloadManagement
{
    public Task<IReadOnlyList<WorkloadLifecycleResponse>> GetAllAsync(CancellationToken token = default) =>
        client.GetAsync<IReadOnlyList<WorkloadLifecycleResponse>>("api/workloads/management", token);
    public async Task<WorkloadLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default)
    {
        try
        {
            return await client.GetAsync<WorkloadLifecycleResponse?>($"api/workloads/management/{id}/lifecycle", token);
        }
        catch (AthenaApiException exception) when (exception.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
    public Task<WorkloadLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default) =>
        client.PostAsync<WorkloadLifecycleResponse>($"api/workloads/management/{id}/lifecycle", token);
    public Task<WorkloadLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) =>
        client.PatchAsync<SetWorkloadDesiredStateRequest, WorkloadLifecycleResponse>(
            $"api/workloads/management/{id}/lifecycle/desired-state", new SetWorkloadDesiredStateRequest(state), token);
    public Task<WorkloadOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default) =>
        client.PostAsync<WorkloadOperationResponse>($"api/workloads/management/{id}/operations/{operation}", token);
    public Task<IReadOnlyList<WorkloadOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default) =>
        client.GetAsync<IReadOnlyList<WorkloadOperationResponse>>($"api/workloads/management/{id}/operations", token);
}
