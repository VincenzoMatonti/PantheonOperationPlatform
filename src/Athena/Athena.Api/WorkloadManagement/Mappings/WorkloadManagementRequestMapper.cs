using Athena.Api.WorkloadManagement.Requests;
using Athena.Application.WorkloadManagement.Commands;
using Athena.Application.WorkloadManagement.Queries;
using Athena.Domain.Workload.Management.Enums;

namespace Athena.Api.WorkloadManagement.Mappings;

public static class WorkloadManagementRequestMapper
{
    public static CreateWorkloadLifecycleCommand ToCreateLifecycleCommand(Guid workloadId) => new(workloadId);
    public static GetWorkloadLifecycleQuery ToGetLifecycleQuery(Guid workloadId) => new(workloadId);
    public static GetWorkloadLifecyclesQuery ToGetLifecyclesQuery() => new();
    public static SetWorkloadDesiredStateCommand ToSetDesiredStateCommand(Guid workloadId, SetWorkloadDesiredStateRequest request) =>
        new(workloadId, request.DesiredState);
    public static ExecuteWorkloadOperationCommand ToExecuteOperationCommand(Guid workloadId, WorkloadOperationType operationType) =>
        new(workloadId, operationType.ToString());
    public static GetWorkloadOperationsQuery ToGetOperationsQuery(Guid workloadId) => new(workloadId);
}
