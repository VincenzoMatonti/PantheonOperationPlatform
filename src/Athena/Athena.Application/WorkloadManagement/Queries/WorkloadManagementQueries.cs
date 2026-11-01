namespace Athena.Application.WorkloadManagement.Queries;

public sealed record GetWorkloadLifecycleQuery(Guid WorkloadId);
public sealed record GetWorkloadLifecyclesQuery;
public sealed record GetWorkloadOperationsQuery(Guid WorkloadId);
