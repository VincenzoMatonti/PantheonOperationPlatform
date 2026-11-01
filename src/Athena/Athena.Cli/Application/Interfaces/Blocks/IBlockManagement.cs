using Athena.Cli.Application.Contracts.Blocks;

namespace Athena.Cli.Application.Interfaces.Blocks;

public interface IBlockManagement
{
    Task<IReadOnlyList<BlockLifecycleResponse>> GetAllAsync(CancellationToken token = default);
    Task<BlockLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default);
    Task<BlockLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default);
    Task<BlockLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default);
    Task<BlockOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default);
    Task<IReadOnlyList<BlockOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default);
}
