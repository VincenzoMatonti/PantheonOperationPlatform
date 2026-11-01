using Athena.Cli.Application.Contracts.Blocks;
using Athena.Cli.Application.Interfaces.Blocks;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Blocks;

public sealed class AthenaBlockManagementClient(AthenaApiClient client) : IBlockManagement
{
    public Task<IReadOnlyList<BlockLifecycleResponse>> GetAllAsync(CancellationToken token = default) =>
        client.GetAsync<IReadOnlyList<BlockLifecycleResponse>>("api/blocks/management", token);
    public Task<BlockLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default) =>
        client.GetAsync<BlockLifecycleResponse?>($"api/blocks/management/{id}/lifecycle", token);
    public Task<BlockLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default) =>
        client.PostAsync<BlockLifecycleResponse>($"api/blocks/management/{id}/lifecycle", token);
    public Task<BlockLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) =>
        client.PatchAsync<SetBlockDesiredStateRequest, BlockLifecycleResponse>(
            $"api/blocks/management/{id}/lifecycle/desired-state", new SetBlockDesiredStateRequest(state), token);
    public Task<BlockOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default) =>
        client.PostAsync<BlockOperationResponse>($"api/blocks/management/{id}/operations/{operation}", token);
    public Task<IReadOnlyList<BlockOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default) =>
        client.GetAsync<IReadOnlyList<BlockOperationResponse>>($"api/blocks/management/{id}/operations", token);
}
