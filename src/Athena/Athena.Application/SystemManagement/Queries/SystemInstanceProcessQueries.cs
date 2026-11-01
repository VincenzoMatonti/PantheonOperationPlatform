namespace Athena.Application.SystemManagement.Queries;

public sealed record GetSystemInstancesQuery;
public sealed record GetSystemInstanceByIdQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceStatusQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceHealthQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceLogsQuery(Guid SystemInstanceId, int Tail);
public sealed record GetSystemInstanceOperationsQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceOperationQuery(Guid SystemInstanceId, Guid OperationId);
