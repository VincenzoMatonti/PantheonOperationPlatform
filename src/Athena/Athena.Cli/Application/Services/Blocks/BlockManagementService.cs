using Athena.Cli.Application.Contracts.Blocks;
using Athena.Cli.Application.Interfaces.Blocks;

namespace Athena.Cli.Application.Services.Blocks;

public sealed class BlockManagementService(IBlockManagement management)
{
    private readonly IBlockManagement _management = management;
    public Task<IReadOnlyList<BlockLifecycleResponse>> GetAllAsync(CancellationToken token = default) => _management.GetAllAsync(token);
    public Task<BlockLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default) => _management.GetAsync(id, token);
    public Task<BlockLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default) => _management.CreateAsync(id, token);
    public Task<BlockLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) => _management.SetDesiredStateAsync(id, state, token);
    public Task<BlockOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default) => _management.ExecuteAsync(id, operation, token);
    public Task<IReadOnlyList<BlockOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default) => _management.GetOperationsAsync(id, token);
}
