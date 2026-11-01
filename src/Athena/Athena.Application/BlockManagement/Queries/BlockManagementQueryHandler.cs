using Athena.Application.BlockManagement.Contracts;
using Athena.Domain.Block.Management.Repositories;
using Athena.Domain.Block.Registry.Exceptions;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;

namespace Athena.Application.BlockManagement.Queries;

public sealed class BlockManagementQueryHandler(
    IBlockRepository blockRepository,
    IBlockLifecycleRepository lifecycleRepository,
    IBlockOperationRepository operationRepository)
{
    public async Task<BlockLifecycleDto?> GetLifecycleAsync(GetBlockLifecycleQuery query, CancellationToken token)
    {
        var id = BlockId.Create(query.BlockId);
        _ = await blockRepository.GetByIdAsync(id, token) ?? throw new BlockNotFoundException(id.Value);
        var lifecycle = await lifecycleRepository.GetByIdAsync(id, token);
        return lifecycle is null ? null : BlockManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<IReadOnlyList<BlockLifecycleDto>> GetLifecyclesAsync(GetBlockLifecyclesQuery query, CancellationToken token) =>
        (await lifecycleRepository.GetAllAsync(token)).Select(BlockManagementDtoMapper.ToDto).ToArray();

    public async Task<IReadOnlyList<BlockOperationDto>> GetOperationsAsync(GetBlockOperationsQuery query, CancellationToken token)
    {
        var id = BlockId.Create(query.BlockId);
        _ = await blockRepository.GetByIdAsync(id, token) ?? throw new BlockNotFoundException(id.Value);
        return (await operationRepository.GetByBlockIdAsync(id, token)).Select(BlockManagementDtoMapper.ToDto).ToArray();
    }
}
