using Athena.Cli.Application.Contracts.Ecosystems;
using Athena.Cli.Application.Interfaces.Ecosystems;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Ecosystems;

public sealed class AthenaEcosystemManagementClient(AthenaApiClient client) : IEcosystemManagement
{
    public Task<IReadOnlyList<EcosystemLifecycleResponse>> GetAllAsync(CancellationToken token = default) =>
        client.GetAsync<IReadOnlyList<EcosystemLifecycleResponse>>("api/ecosystems/management", token);
    public Task<EcosystemLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default) =>
        client.GetAsync<EcosystemLifecycleResponse?>($"api/ecosystems/management/{id}/lifecycle", token);
    public Task<EcosystemLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default) =>
        client.PostAsync<EcosystemLifecycleResponse>($"api/ecosystems/management/{id}/lifecycle", token);
    public Task<EcosystemLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) =>
        client.PatchAsync<SetEcosystemDesiredStateRequest, EcosystemLifecycleResponse>(
            $"api/ecosystems/management/{id}/lifecycle/desired-state", new SetEcosystemDesiredStateRequest(state), token);
    public Task<EcosystemOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default) =>
        client.PostAsync<EcosystemOperationResponse>($"api/ecosystems/management/{id}/operations/{operation}", token);
    public Task<IReadOnlyList<EcosystemOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default) =>
        client.GetAsync<IReadOnlyList<EcosystemOperationResponse>>($"api/ecosystems/management/{id}/operations", token);
}
