using Athena.Application.BlockManagement.Contracts;
using Athena.Application.BlockManagement.Exceptions;
using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Management.Enums;
using Athena.Domain.Block.Management.Repositories;
using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.Exceptions;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;

namespace Athena.Application.BlockManagement.Commands;

public sealed class BlockManagementCommandHandler(IBlockRepository blockRepository, IBlockLifecycleRepository lifecycleRepository, IBlockOperationRepository operationRepository)
{
    public async Task<BlockLifecycleDto> CreateLifecycleAsync(CreateBlockLifecycleCommand command, CancellationToken token)
    {
        var id = BlockId.Create(command.BlockId);
        _ = await GetBlockAsync(id, token);
        if (await lifecycleRepository.GetByIdAsync(id, token) is not null)
            throw Error(BlockManagementErrorCode.BlockLifecycleAlreadyExists, $"Lifecycle for block '{id.Value}' already exists.");
        var lifecycle = BlockLifecycle.Create(id);
        await lifecycleRepository.AddAsync(lifecycle, token);
        return BlockManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<BlockLifecycleDto> SetDesiredStateAsync(SetBlockDesiredStateCommand command, CancellationToken token)
    {
        if (!Enum.TryParse<BlockDesiredState>(command.DesiredState, true, out var desired) || !Enum.IsDefined(desired))
            throw Error(BlockManagementErrorCode.InvalidDesiredState, $"Desired state '{command.DesiredState}' is invalid.");
        var id = BlockId.Create(command.BlockId);
        EnsureRegistered(await GetBlockAsync(id, token));
        var lifecycle = await GetRequiredLifecycleAsync(id, token);
        lifecycle.SetDesiredState(desired);
        lifecycle.SetOperationalStatus(BlockOperationalStatus.Unknown);
        await lifecycleRepository.UpdateAsync(lifecycle, token);
        return BlockManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<BlockOperationDto> ExecuteOperationAsync(ExecuteBlockOperationCommand command, CancellationToken token)
    {
        if (!Enum.TryParse<BlockOperationType>(command.OperationType, true, out var type) || !Enum.IsDefined(type))
            throw Error(BlockManagementErrorCode.InvalidOperationType, $"Operation type '{command.OperationType}' is invalid.");
        var id = BlockId.Create(command.BlockId);
        EnsureRegistered(await GetBlockAsync(id, token));
        var lifecycle = await GetRequiredLifecycleAsync(id, token);
        var operation = BlockOperation.Create(Guid.NewGuid(), id, type);
        lifecycle.SetDesiredState(type switch
        {
            BlockOperationType.Start or BlockOperationType.Restart => BlockDesiredState.Running,
            BlockOperationType.Stop => BlockDesiredState.Stopped,
            _ => throw Error(BlockManagementErrorCode.InvalidOperationType, $"Operation type '{command.OperationType}' is invalid.")
        });
        lifecycle.SetOperationalStatus(BlockOperationalStatus.Unknown);
        operation.Start();
        operation.Complete();
        await operationRepository.SaveIntentAsync(operation, lifecycle, token);
        return BlockManagementDtoMapper.ToDto(operation);
    }

    private async Task<Block> GetBlockAsync(BlockId id, CancellationToken token) =>
        await blockRepository.GetByIdAsync(id, token) ?? throw new BlockNotFoundException(id.Value);

    private async Task<BlockLifecycle> GetRequiredLifecycleAsync(BlockId id, CancellationToken token) =>
        await lifecycleRepository.GetByIdAsync(id, token) ?? throw Error(BlockManagementErrorCode.BlockLifecycleNotFound, $"Lifecycle for block '{id.Value}' was not found.");

    private static void EnsureRegistered(Block block)
    {
        if (block.RegistrationStatus == BlockRegistrationStatus.Disabled) throw new BlockAlreadyDisabledException(block.Id.Value);
        if (block.RegistrationStatus == BlockRegistrationStatus.Unregistered) throw new BlockNotRegisteredException(block.Id.Value);
    }

    private static BlockManagementException Error(BlockManagementErrorCode code, string message) => new(code, message);

}
