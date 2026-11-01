using Athena.Api.BlockManagement.Requests;
using Athena.Application.BlockManagement.Commands;
using Athena.Application.BlockManagement.Queries;
using Athena.Domain.Block.Management.Enums;

namespace Athena.Api.BlockManagement.Mappings;

public static class BlockManagementRequestMapper
{
    public static CreateBlockLifecycleCommand ToCreateLifecycleCommand(Guid blockId) => new(blockId);
    public static GetBlockLifecycleQuery ToGetLifecycleQuery(Guid blockId) => new(blockId);
    public static GetBlockLifecyclesQuery ToGetLifecyclesQuery() => new();
    public static SetBlockDesiredStateCommand ToSetDesiredStateCommand(Guid blockId, SetBlockDesiredStateRequest request) =>
        new(blockId, request.DesiredState);
    public static ExecuteBlockOperationCommand ToExecuteOperationCommand(Guid blockId, BlockOperationType operationType) =>
        new(blockId, operationType.ToString());
    public static GetBlockOperationsQuery ToGetOperationsQuery(Guid blockId) => new(blockId);
}
