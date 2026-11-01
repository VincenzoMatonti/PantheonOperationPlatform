using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Registry.ValueObjects;

namespace Athena.Domain.Block.Management.Repositories;

public interface IBlockLifecycleRepository
{
    Task<BlockLifecycle?> GetByIdAsync(BlockId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(BlockLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(BlockLifecycle lifecycle, CancellationToken cancellationToken = default);
}

public interface IBlockOperationRepository
{
    Task AddAsync(BlockOperation operation, CancellationToken cancellationToken = default);
    Task SaveIntentAsync(BlockOperation operation, BlockLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(BlockOperation operation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockOperation>> GetByBlockIdAsync(BlockId id, CancellationToken cancellationToken = default);
}
