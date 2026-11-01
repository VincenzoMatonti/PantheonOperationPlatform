using Athena.Application.BlockRegistry.Contracts;
using Athena.Application.BlockRegistry.Enums;
using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Application.BlockRegistry.Queries;

public sealed class BlockRegistryQueryHandler(IBlockRepository blockRepository)
{
    private readonly IBlockRepository _blockRepository = blockRepository;

    public async Task<BlockDto?> GetByIdAsync(GetBlockByIdQuery query, CancellationToken cancellationToken = default)
    {
        var block = await _blockRepository.GetByIdAsync(BlockId.Create(query.BlockId), cancellationToken);
        return block is null ? null : BlockRegistryDtoMapper.ToDto(block);
    }

    public async Task<BlockDto?> GetByNameAsync(GetBlockByNameQuery query, CancellationToken cancellationToken = default)
    {
        var block = await _blockRepository.GetByNameAsync(BlockName.Create(query.Name), cancellationToken);
        return block is null ? null : BlockRegistryDtoMapper.ToDto(block);
    }

    public async Task<IReadOnlyList<BlockDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var blocks = await _blockRepository.GetAllAsync(cancellationToken);
        return BlockRegistryDtoMapper.ToDtos(blocks);
    }

    public async Task<IReadOnlyList<BlockDto>> GetByEcosystemIdAsync(GetBlocksByEcosystemIdQuery query, CancellationToken cancellationToken = default)
    {
        var blocks = await _blockRepository.GetByEcosystemIdAsync(EcosystemId.Create(query.EcosystemId), cancellationToken);
        return BlockRegistryDtoMapper.ToDtos(blocks);
    }

    public async Task<IReadOnlyList<BlockDto>> GetByRegistrationStatusAsync(GetBlocksByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            BlockRegistrationStatusFilter.Registered => BlockRegistrationStatus.Registered,
            BlockRegistrationStatusFilter.Unregistered => BlockRegistrationStatus.Unregistered,
            BlockRegistrationStatusFilter.Disabled => BlockRegistrationStatus.Disabled,
            _ => (BlockRegistrationStatus?)null
        };

        if (status is null) return [];
        var blocks = await _blockRepository.GetByRegistrationStatusAsync(status.Value, cancellationToken);
        return BlockRegistryDtoMapper.ToDtos(blocks);
    }

    public async Task<IReadOnlyList<BlockDto>> GetByAvailabilityStatusAsync(GetBlocksByAvailabilityStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            BlockAvailabilityStatusFilter.Available => BlockAvailabilityStatus.Available,
            BlockAvailabilityStatusFilter.Unavailable => BlockAvailabilityStatus.Unavailable,
            BlockAvailabilityStatusFilter.Unknown => BlockAvailabilityStatus.Unknown,
            BlockAvailabilityStatusFilter.Error => BlockAvailabilityStatus.Error,
            _ => (BlockAvailabilityStatus?)null
        };

        if (status is null) return [];
        var blocks = await _blockRepository.GetByAvailabilityStatusAsync(status.Value, cancellationToken);
        return BlockRegistryDtoMapper.ToDtos(blocks);
    }

    public async Task<IReadOnlyList<BlockDto>> GetByDeletionStatusAsync(GetBlocksByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var blocks = await _blockRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return BlockRegistryDtoMapper.ToDtos(blocks);
    }

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _blockRepository.ExistsByNameAsync(BlockName.Create(name), cancellationToken);
    }
}