using Athena.Domain.Workload.Management.Entities;

namespace Athena.Application.WorkloadManagement.Contracts;

public static class WorkloadManagementDtoMapper
{
    public static WorkloadLifecycleDto ToDto(WorkloadLifecycle lifecycle) =>
        new(lifecycle.Id.Value, lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(),
            lifecycle.CreatedAtUtc, lifecycle.UpdatedAtUtc, lifecycle.IsDeleted);

    public static WorkloadOperationDto ToDto(WorkloadOperation operation) =>
        new(operation.Id, operation.WorkloadId.Value, operation.Type.ToString(), operation.Status.ToString(),
            operation.CreatedAtUtc, operation.StartedAtUtc, operation.CompletedAtUtc,
            operation.ErrorMessage, operation.IsDeleted);
}
