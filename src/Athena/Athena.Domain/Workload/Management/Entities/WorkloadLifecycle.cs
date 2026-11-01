using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.Workload.Management.Entities;

public sealed class WorkloadLifecycle
{
    public WorkloadId Id { get; private set; } = null!;
    public WorkloadDesiredState DesiredState { get; private set; }
    public WorkloadOperationalStatus OperationalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static WorkloadLifecycle Create(WorkloadId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return new WorkloadLifecycle { Id = id, CreatedAtUtc = DateTime.UtcNow };
    }

    public void SetDesiredState(WorkloadDesiredState state)
    {
        EnsureActive();
        if (!Enum.IsDefined(state)) throw new WorkloadManagementException(WorkloadManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.", DomainErrorType.Validation);
        if (DesiredState == state) return;
        DesiredState = state;
        Touch();
    }

    public void SetOperationalStatus(WorkloadOperationalStatus status)
    {
        EnsureActive();
        if (!Enum.IsDefined(status)) throw new WorkloadManagementException(WorkloadManagementErrorCode.InvalidOperationalStatus, $"Operational status '{status}' is invalid.", DomainErrorType.Validation);
        if (OperationalStatus == status) return;
        OperationalStatus = status;
        Touch();
    }

    public void Delete()
    {
        EnsureActive();
        IsDeleted = true;
        Touch();
    }

    private void EnsureActive()
    {
        if (IsDeleted) throw new WorkloadManagementException(WorkloadManagementErrorCode.LifecycleDeletedCannotBeModified, $"Workload lifecycle '{Id.Value}' is deleted.");
    }

    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
