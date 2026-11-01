using Athena.Application.BlockRegistry.Contracts;
using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using BlockEntity = Athena.Domain.Block.Registry.Entities.Block;

namespace Athena.Application.BlockRegistry.Commands;

public sealed class BlockRegistryCommandHandler(IBlockRepository blockRepository)
{
    private readonly IBlockRepository _blockRepository = blockRepository;

    public async Task<BlockDto> CreateAsync(CreateBlockCommand command, CancellationToken cancellationToken = default)
    {
        var id = BlockId.New();
        var block = BlockEntity.Create(id, EcosystemId.Create(command.EcosystemId), BlockName.Create(command.Name));
        var lifecycle = BlockLifecycle.Create(id);
        await _blockRepository.AddAsync(block, lifecycle, cancellationToken);
        return BlockRegistryDtoMapper.ToDto(block);
    }

    public Task<BlockDto?> RegisterAsync(RegisterBlockCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.Register(), cancellationToken);

    public Task<BlockDto?> UnregisterAsync(UnregisterBlockCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.Unregister(), cancellationToken);

    public Task<BlockDto?> RenameAsync(RenameBlockCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.BlockId, block => block.Rename(BlockName.Create(command.NewName)), cancellationToken);

    public Task<BlockDto?> EnableAsync(EnableBlockCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.Enable(), cancellationToken);

    public Task<BlockDto?> DisableAsync(DisableBlockCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.Disable(), cancellationToken);

    public Task<BlockDto?> DeleteAsync(DeleteBlockCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.Delete(), cancellationToken);

    public Task<BlockDto?> MarkAvailableAsync(MarkBlockAvailableCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.MarkAvailable(), cancellationToken);

    public Task<BlockDto?> MarkUnavailableAsync(MarkBlockUnavailableCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.BlockId, block => block.MarkUnavailable(), cancellationToken);

    public Task<BlockDto?> MarkAvailabilityUnknownAsync(MarkBlockAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.BlockId, block => block.MarkAvailabilityUnknown(), cancellationToken);

    public Task<BlockDto?> MarkErrorAsync(MarkBlockErrorCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.BlockId, block => block.MarkError(), cancellationToken);

    private async Task<BlockDto?> MutateAsync(Guid blockId, Action<BlockEntity> mutation, CancellationToken cancellationToken)
    {
        var block = await _blockRepository.GetByIdAsync(BlockId.Create(blockId), cancellationToken);
        if (block is null) return null;
        mutation(block);
        await _blockRepository.UpdateAsync(block, cancellationToken);
        return BlockRegistryDtoMapper.ToDto(block);
    }
}