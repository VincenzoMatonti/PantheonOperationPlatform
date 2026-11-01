namespace Athena.Application.WorkloadManagement.Commands;

public sealed record CreateWorkloadLifecycleCommand(Guid WorkloadId);
public sealed record SetWorkloadDesiredStateCommand(Guid WorkloadId, string DesiredState);
public sealed record ExecuteWorkloadOperationCommand(Guid WorkloadId, string OperationType);
