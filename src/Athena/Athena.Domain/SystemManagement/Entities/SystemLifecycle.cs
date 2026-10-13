using Athena.Domain.SystemManagement.Enums;
using Athena.Domain.SystemManagement.Exceptions;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.SystemManagement.Entities;

public class SystemLifecycle
{
    public SystemId SystemId { get; private set; } = null!;
    public SystemDesiredState DesiredState { get; private set; }
    public SystemOperationalStatus OperationalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static SystemLifecycle Create(SystemId systemId)
    {
        if (systemId == null) throw new SystemLifecycleSystemIdRequiredException();
        return new SystemLifecycle
        {
            SystemId = systemId,
            DesiredState = SystemDesiredState.Unknown,
            OperationalStatus = SystemOperationalStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void SetDesiredState(SystemDesiredState desiredState)
    {
        EnsureNotDeleted();
        if (!Enum.IsDefined(desiredState)) throw new SystemLifecycleInvalidDesiredStateException();
        if (DesiredState == desiredState) throw new SystemLifecycleDesiredStateAlreadySetException(SystemId.Value, desiredState.ToString());
        DesiredState = desiredState;
        Touch();
    }

    public void MarkStarting()
    {
        EnsureNotDeleted();
        if (OperationalStatus == SystemOperationalStatus.Starting) throw new SystemLifecycleAlreadyStartingException(SystemId.Value);
        if (OperationalStatus != SystemOperationalStatus.Unknown &&
            OperationalStatus != SystemOperationalStatus.Stopped &&
            OperationalStatus != SystemOperationalStatus.Failed)
        {
            throw new SystemLifecycleInvalidOperationalTransitionException(SystemId.Value, OperationalStatus, SystemOperationalStatus.Starting);
        }

        OperationalStatus = SystemOperationalStatus.Starting;
        Touch();
    }

    public void MarkRunning()
    {
        EnsureNotDeleted();
        if (OperationalStatus == SystemOperationalStatus.Running) throw new SystemLifecycleAlreadyRunningException(SystemId.Value);
        EnsureOperationalStatus(SystemOperationalStatus.Starting);
        OperationalStatus = SystemOperationalStatus.Running;
        Touch();
    }

    public void MarkStopping()
    {
        EnsureNotDeleted();
        if (OperationalStatus == SystemOperationalStatus.Stopping) throw new SystemLifecycleAlreadyStoppingException(SystemId.Value);
        EnsureOperationalStatus(SystemOperationalStatus.Running);
        OperationalStatus = SystemOperationalStatus.Stopping;
        Touch();
    }

    public void MarkStopped()
    {
        EnsureNotDeleted();
        if (OperationalStatus == SystemOperationalStatus.Stopped) throw new SystemLifecycleAlreadyStoppedException(SystemId.Value);
        EnsureOperationalStatus(SystemOperationalStatus.Stopping);
        OperationalStatus = SystemOperationalStatus.Stopped;
        Touch();
    }

    public void MarkFailed()
    {
        EnsureNotDeleted();
        if (OperationalStatus == SystemOperationalStatus.Failed) throw new SystemLifecycleAlreadyFailedException(SystemId.Value);
        OperationalStatus = SystemOperationalStatus.Failed;
        Touch();
    }

    public void MarkUnknown()
    {
        EnsureNotDeleted();
        if (OperationalStatus == SystemOperationalStatus.Unknown) throw new SystemLifecycleAlreadyUnknownException(SystemId.Value);
        OperationalStatus = SystemOperationalStatus.Unknown;
        Touch();
    }

    public void Delete()
    {
        if (IsDeleted) throw new SystemLifecycleAlreadyDeletedException(SystemId.Value);
        if (OperationalStatus == SystemOperationalStatus.Starting || OperationalStatus == SystemOperationalStatus.Stopping)
            throw new SystemLifecycleCannotBeDeletedWhileTransitioningException(SystemId.Value, OperationalStatus);

        IsDeleted = true;
        Touch();
    }

    private void EnsureOperationalStatus(SystemOperationalStatus expectedStatus)
    {
        if (OperationalStatus != expectedStatus) throw new SystemLifecycleInvalidOperationalTransitionException(SystemId.Value, OperationalStatus, expectedStatus);

    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new SystemLifecycleDeletedCannotBeModifiedException(SystemId.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}