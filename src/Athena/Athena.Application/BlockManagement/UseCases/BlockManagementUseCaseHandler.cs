using Athena.Application.BlockManagement.Contracts;
using Athena.Application.BlockManagement.Exceptions;
using Athena.Application.Common.Services;
using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Management.Enums;
using Athena.Domain.Block.Management.Repositories;
using Athena.Domain.Block.Registry.Exceptions;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;

namespace Athena.Application.BlockManagement.UseCases;

public sealed class BlockManagementUseCaseHandler(
    IBlockRepository blockRepository,
    IBlockLifecycleRepository lifecycleRepository,
    IBlockOperationRepository operationRepository,
    ApplicationExceptionService exceptionService) : IBlockManagement
{
    public Task<BlockLifecycleDto> CreateLifecycleAsync(Guid id, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            var blockId = BlockId.Create(id);
            await GetBlockAsync(blockId, token);
            if (await lifecycleRepository.GetByIdAsync(blockId, token) is not null)
                throw Error(BlockManagementErrorCode.BlockLifecycleAlreadyExists, $"Lifecycle for block '{id}' already exists.");
            var lifecycle = BlockLifecycle.Create(blockId);
            await lifecycleRepository.AddAsync(lifecycle, token);
            return ToDto(lifecycle);
        });
    public Task<BlockLifecycleDto?> GetLifecycleAsync(Guid id, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            var blockId = BlockId.Create(id);
            await GetBlockAsync(blockId, token);
            var lifecycle = await lifecycleRepository.GetByIdAsync(blockId, token);
            return lifecycle is null ? null : ToDto(lifecycle);
        });
    public Task<IReadOnlyList<BlockLifecycleDto>> GetLifecyclesAsync(CancellationToken token = default) =>
        RunAsync<IReadOnlyList<BlockLifecycleDto>>(async () => (await lifecycleRepository.GetAllAsync(token)).Select(ToDto).ToArray());
    public Task<BlockLifecycleDto> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            if (!Enum.TryParse<BlockDesiredState>(state, true, out var desired) || !Enum.IsDefined(desired))
                throw Error(BlockManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.");
            var blockId = BlockId.Create(id);
            EnsureRegistered(await GetBlockAsync(blockId, token));
            var lifecycle = await GetRequiredLifecycleAsync(blockId, token);
            lifecycle.SetDesiredState(desired);
            lifecycle.SetOperationalStatus(BlockOperationalStatus.Unknown);
            await lifecycleRepository.UpdateAsync(lifecycle, token);
            return ToDto(lifecycle);
        });
    public Task<BlockOperationDto> ExecuteOperationAsync(Guid id, string type, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            if (!Enum.TryParse<BlockOperationType>(type, true, out var operationType) || !Enum.IsDefined(operationType))
                throw Error(BlockManagementErrorCode.InvalidOperationType, $"Operation type '{type}' is invalid.");
            var blockId = BlockId.Create(id);
            EnsureRegistered(await GetBlockAsync(blockId, token));
            var lifecycle = await GetRequiredLifecycleAsync(blockId, token);
            var operation = BlockOperation.Create(Guid.NewGuid(), blockId, operationType);
            var desired = operationType switch
            {
                BlockOperationType.Start or BlockOperationType.Restart => BlockDesiredState.Running,
                BlockOperationType.Stop => BlockDesiredState.Stopped,
                _ => throw Error(BlockManagementErrorCode.InvalidOperationType, $"Operation type '{type}' is invalid.")
            };
            lifecycle.SetDesiredState(desired);
            lifecycle.SetOperationalStatus(BlockOperationalStatus.Unknown);
            operation.Start();
            operation.Complete();
            await operationRepository.SaveIntentAsync(operation, lifecycle, token);
            return ToDto(operation);
        });
    public Task<IReadOnlyList<BlockOperationDto>> GetOperationsAsync(Guid id, CancellationToken token = default) =>
        RunAsync<IReadOnlyList<BlockOperationDto>>(async () =>
        {
            var blockId = BlockId.Create(id);
            await GetBlockAsync(blockId, token);
            return (await operationRepository.GetByBlockIdAsync(blockId, token)).Select(ToDto).ToArray();
        });
    private async Task<Athena.Domain.Block.Registry.Entities.Block> GetBlockAsync(BlockId id, CancellationToken token) =>
        await blockRepository.GetByIdAsync(id, token) ?? throw new BlockNotFoundException(id.Value);
    private static void EnsureRegistered(Athena.Domain.Block.Registry.Entities.Block block)
    {
        if (block.RegistrationStatus == Athena.Domain.Block.Registry.Enums.BlockRegistrationStatus.Disabled)
            throw new Athena.Domain.Block.Registry.Exceptions.BlockAlreadyDisabledException(block.Id.Value);
        if (block.RegistrationStatus == Athena.Domain.Block.Registry.Enums.BlockRegistrationStatus.Unregistered)
            throw new BlockNotRegisteredException(block.Id.Value);
    }
    private async Task<BlockLifecycle> GetRequiredLifecycleAsync(BlockId id, CancellationToken token) =>
        await lifecycleRepository.GetByIdAsync(id, token) ?? throw Error(BlockManagementErrorCode.BlockLifecycleNotFound, $"Lifecycle for block '{id.Value}' was not found.");
    private Task<T> RunAsync<T>(Func<Task<T>> operation) => exceptionService.ExecuteAsync(operation);
    private static BlockManagementException Error(BlockManagementErrorCode code, string message) => new(code, message);
    private static BlockLifecycleDto ToDto(BlockLifecycle lifecycle) =>
        new(lifecycle.Id.Value, lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(), lifecycle.CreatedAtUtc, lifecycle.UpdatedAtUtc, lifecycle.IsDeleted);
    private static BlockOperationDto ToDto(BlockOperation operation) =>
        new(operation.Id, operation.BlockId.Value, operation.Type.ToString(), operation.Status.ToString(),
            operation.CreatedAtUtc, operation.StartedAtUtc, operation.CompletedAtUtc, operation.ErrorMessage, operation.IsDeleted);
}
