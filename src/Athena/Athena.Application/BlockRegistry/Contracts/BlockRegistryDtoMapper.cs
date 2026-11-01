using Athena.Domain.Block.Registry.Entities;
using BlockEntity = Athena.Domain.Block.Registry.Entities.Block;

namespace Athena.Application.BlockRegistry.Contracts;

public static class BlockRegistryDtoMapper
{
    public static BlockDto ToDto(BlockEntity block)
    {
        return new BlockDto
        {
            Id = block.Id.Value,
            EcosystemId = block.EcosystemId.Value,
            Name = block.Name.Value,
            RegistrationStatus = block.RegistrationStatus.ToString(),
            AvailabilityStatus = block.AvailabilityStatus.ToString(),
            CreatedAtUtc = block.CreatedAtUtc,
            UpdatedAtUtc = block.UpdatedAtUtc,
            IsDeleted = block.IsDeleted
        };
    }

    public static IReadOnlyList<BlockDto> ToDtos(IEnumerable<BlockEntity> blocks)
    {
        return [.. blocks.Select(ToDto)];
    }
}