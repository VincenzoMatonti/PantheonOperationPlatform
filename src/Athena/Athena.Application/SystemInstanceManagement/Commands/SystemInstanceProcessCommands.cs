using Athena.Application.SystemInstanceManagement.Contracts;
using Athena.Domain.System.Management.Enums;

namespace Athena.Application.SystemInstanceManagement.Commands;

public sealed record CreateSystemInstanceCommand(SystemInstanceExecutionCommandRequest Request);
public sealed record RegisterSystemInstanceCommand(SystemInstanceExecutionCommandRequest Request);
public sealed record StartSystemInstanceCommand(Guid SystemInstanceId);
public sealed record StopSystemInstanceCommand(Guid SystemInstanceId);
public sealed record RestartSystemInstanceCommand(Guid SystemInstanceId);
public sealed record RemoveSystemInstanceCommand(Guid SystemInstanceId);
public sealed record EnqueueSystemInstanceOperationCommand(Guid SystemInstanceId, SystemInstanceOperationType Type);
public sealed record RemoveSystemInstanceOperationCommand(Guid SystemInstanceId);
public sealed record UnregisterSystemInstanceOperationCommand(Guid SystemInstanceId);
public sealed record ConfigureSystemInstanceCommand(Guid SystemInstanceId, string ConfigurationReference);
public sealed record ResolveSystemInstanceOperationCommand(Guid SystemInstanceId, Guid OperationId, string Reason);