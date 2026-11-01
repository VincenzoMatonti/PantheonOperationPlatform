namespace Athena.Application.SystemInstanceRegistry.Commands;

public sealed record CreateSystemInstanceCommand(Guid SystemInstanceId, Guid SystemId, Guid WorkloadId, string Name);
public sealed record RenameSystemInstanceCommand(Guid SystemInstanceId, string Name);
public sealed record RegisterSystemInstanceCommand(Guid SystemInstanceId);
public sealed record UnregisterSystemInstanceCommand(Guid SystemInstanceId);
public sealed record EnableSystemInstanceCommand(Guid SystemInstanceId);
public sealed record DisableSystemInstanceCommand(Guid SystemInstanceId);
public sealed record DeleteSystemInstanceCommand(Guid SystemInstanceId);