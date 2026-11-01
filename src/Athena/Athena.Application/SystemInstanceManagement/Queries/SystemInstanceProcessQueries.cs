using Athena.Domain.System.Management.Enums;

namespace Athena.Application.SystemInstanceManagement.Queries;

public sealed record GetSystemInstancesQuery;
public sealed record GetSystemInstanceByIdQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceStatusQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceHealthQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceLogsQuery(Guid SystemInstanceId, int Tail);

public sealed record GetSystemInstanceOperationsQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceOperationQuery(Guid SystemInstanceId, Guid OperationId);
public sealed record GetSystemInstancePendingOperationsQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceRunningOperationsQuery(Guid SystemInstanceId);

public sealed record GetSystemInstanceLifecycleQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceLifecyclesQuery;
public sealed record GetSystemInstancesByDesiredStateQuery(SystemInstanceDesiredState DesiredState);
public sealed record GetSystemInstancesByOperationalStatusQuery(SystemInstanceOperationalStatus OperationalStatus);

public sealed record HasActiveSystemInstanceOperationQuery(Guid SystemInstanceId);