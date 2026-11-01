using Athena.Application.SystemManagement.Contracts;
using Athena.Domain.System.Management.Enums;

namespace Athena.Application.SystemManagement.Commands;

public sealed record CreateSystemInstanceCommand(SystemInstanceExecutionCommandRequest Request);
public sealed record RegisterSystemInstanceCommand(SystemInstanceExecutionCommandRequest Request);
public sealed record EnqueueSystemInstanceOperationCommand(Guid SystemInstanceId, SystemInstanceOperationType Type);
public sealed record ConfigureSystemInstanceCommand(Guid SystemInstanceId, string ConfigurationReference);
public sealed record ResolveSystemInstanceOperationCommand(Guid SystemInstanceId, Guid OperationId, string Reason);
