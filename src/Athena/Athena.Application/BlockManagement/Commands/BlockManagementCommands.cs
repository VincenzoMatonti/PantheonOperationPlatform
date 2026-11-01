namespace Athena.Application.BlockManagement.Commands;

public sealed record CreateBlockLifecycleCommand(Guid BlockId);
public sealed record SetBlockDesiredStateCommand(Guid BlockId, string DesiredState);
public sealed record ExecuteBlockOperationCommand(Guid BlockId, string OperationType);
