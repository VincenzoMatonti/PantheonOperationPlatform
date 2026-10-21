
using Athena.Domain.OrchestrationManagement.Enums;
using Athena.Domain.OrchestrationManagement.Exceptions;
using Athena.Domain.OrchestrationRegistry.ValueObjects;

namespace Athena.Domain.OrchestrationManagement.Entities;

public class OrchestrationLifecycle
{
    public OrchestrationId Id { get; private set; } = null!;
    public OrchestrationDesiredState DesiredState { get; private set; }
    public OrchestrationOperationalStatus OperationalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static OrchestrationLifecycle Create(OrchestrationId id)
    {
        if (id == null) throw new OrchestrationLifecycleIdRequiredException();
        return new OrchestrationLifecycle
        {
            Id = id,
            DesiredState = OrchestrationDesiredState.Unknown,
            OperationalStatus = OrchestrationOperationalStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void SetDesiredState(OrchestrationDesiredState state)
    {
        EnsureNotDeleted();
        if (!Enum.IsDefined(state)) throw new OrchestrationLifecycleInvalidDesiredStateException();
        if (DesiredState == state) throw new OrchestrationLifecycleDesiredStateAlreadySetException(Id.Value, state);
        DesiredState = state;
        Touch();
    }

    public void MarkStarting()
    {
        SetOperationalStatus(OrchestrationOperationalStatus.Starting);
    }

    public void MarkRunning()
    {
        SetOperationalStatus(OrchestrationOperationalStatus.Running);
    }

    public void MarkStopping()
    {
        SetOperationalStatus(OrchestrationOperationalStatus.Stopping);
    }

    public void MarkStopped()
    {
        SetOperationalStatus(OrchestrationOperationalStatus.Stopped);
    }

    public void MarkFailed()
    {
        SetOperationalStatus(OrchestrationOperationalStatus.Failed);
    }

    public void MarkOperationalStatusUnknown()
    {
        SetOperationalStatus(OrchestrationOperationalStatus.Unknown);
    }

    public void Delete()
    {
        if (IsDeleted) throw new OrchestrationLifecycleAlreadyDeletedException(Id.Value);
        IsDeleted = true;
        Touch();
    }

    private void SetOperationalStatus(OrchestrationOperationalStatus status)
    {
        EnsureNotDeleted();
        if (!Enum.IsDefined(status)) throw new OrchestrationLifecycleInvalidOperationalStatusException();
        if (OperationalStatus == status) throw new OrchestrationLifecycleAlreadyInOperationalStatusException(Id.Value, status);
        OperationalStatus = status;
        Touch();
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new OrchestrationLifecycleDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
