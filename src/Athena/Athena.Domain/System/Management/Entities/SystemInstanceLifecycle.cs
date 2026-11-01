using Athena.Domain.Common;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Exceptions;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Management.Entities;

public sealed class SystemInstanceLifecycle
{
    public SystemInstanceId Id { get; private set; } = null!;
    public SystemInstanceDesiredState DesiredState { get; private set; }
    public SystemInstanceOperationalStatus OperationalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static SystemInstanceLifecycle Create(SystemInstanceId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return new SystemInstanceLifecycle
        {
            Id = id,
            DesiredState = SystemInstanceDesiredState.Unknown,
            OperationalStatus = SystemInstanceOperationalStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void SetDesiredState(SystemInstanceDesiredState state)
    {
        EnsureNotDeleted();
        if (!Enum.IsDefined(state)) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.", DomainErrorType.Validation);
        if (DesiredState == state) return;
        DesiredState = state;
        Touch();
    }

    public void SetOperationalStatus(SystemInstanceOperationalStatus status)
    {
        EnsureNotDeleted();
        if (!Enum.IsDefined(status)) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.InvalidOperationalStatus, $"Operational status '{status}' is invalid.", DomainErrorType.Validation);
        if (OperationalStatus == status) return;
        OperationalStatus = status;
        Touch();
    }

    public void Delete()
    {
        EnsureNotDeleted();
        IsDeleted = true;
        Touch();
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.LifecycleDeletedCannotBeModified, $"SystemInstance lifecycle '{Id.Value}' is deleted.");
    }

    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
