using Athena.Application.BlockManagement.Commands;
using Athena.Application.BlockManagement.Contracts;
using Athena.Application.BlockManagement.Queries;
using Athena.Application.Common.Services;

namespace Athena.Application.BlockManagement.UseCases;

public sealed class BlockManagementUseCaseHandler(
    BlockManagementCommandHandler commandHandler,
    BlockManagementQueryHandler queryHandler,
    ApplicationExceptionService exceptionService) : IBlockManagement
{
    public Task<BlockLifecycleDto> CreateLifecycleAsync(CreateBlockLifecycleCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.CreateLifecycleAsync(command, token));

    public Task<BlockLifecycleDto?> GetLifecycleAsync(GetBlockLifecycleQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetLifecycleAsync(query, token));

    public Task<IReadOnlyList<BlockLifecycleDto>> GetLifecyclesAsync(GetBlockLifecyclesQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetLifecyclesAsync(query, token));

    public Task<BlockLifecycleDto> SetDesiredStateAsync(SetBlockDesiredStateCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.SetDesiredStateAsync(command, token));

    public Task<BlockOperationDto> ExecuteOperationAsync(ExecuteBlockOperationCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.ExecuteOperationAsync(command, token));

    public Task<IReadOnlyList<BlockOperationDto>> GetOperationsAsync(GetBlockOperationsQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetOperationsAsync(query, token));

    public Task<BlockLifecycleDto> CreateLifecycleAsync(Guid blockId, CancellationToken token = default) =>
        CreateLifecycleAsync(new CreateBlockLifecycleCommand(blockId), token);

    public Task<BlockLifecycleDto?> GetLifecycleAsync(Guid blockId, CancellationToken token = default) =>
        GetLifecycleAsync(new GetBlockLifecycleQuery(blockId), token);

    public Task<IReadOnlyList<BlockLifecycleDto>> GetLifecyclesAsync(CancellationToken token = default) =>
        GetLifecyclesAsync(new GetBlockLifecyclesQuery(), token);

    public Task<BlockLifecycleDto> SetDesiredStateAsync(Guid blockId, string desiredState, CancellationToken token = default) =>
        SetDesiredStateAsync(new SetBlockDesiredStateCommand(blockId, desiredState), token);

    public Task<BlockOperationDto> ExecuteOperationAsync(Guid blockId, string operationType, CancellationToken token = default) =>
        ExecuteOperationAsync(new ExecuteBlockOperationCommand(blockId, operationType), token);

    public Task<IReadOnlyList<BlockOperationDto>> GetOperationsAsync(Guid blockId, CancellationToken token = default) =>
        GetOperationsAsync(new GetBlockOperationsQuery(blockId), token);

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => exceptionService.ExecuteAsync(operation);
}
