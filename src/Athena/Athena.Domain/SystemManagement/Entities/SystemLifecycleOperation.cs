using Athena.Domain.SystemManagement.Enums;
using Athena.Domain.SystemManagement.Exceptions;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.SystemManagement.Entities;

public class SystemLifecycleOperation
{
    public Guid Id { get; private set; }
    public SystemId SystemId { get; private set; } = null!;
    public SystemLifecycleOperationType Type { get; private set; }
    public SystemLifecycleOperationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static SystemLifecycleOperation Create(SystemId systemId, SystemLifecycleOperationType type)
    {
        if (systemId == null) throw new SystemLifecycleOperationSystemIdRequiredException();
        if (!Enum.IsDefined(type)) throw new SystemLifecycleOperationInvalidTypeException();
        return new SystemLifecycleOperation
        {
            Id = Guid.NewGuid(),
            SystemId = systemId,
            Type = type,
            Status = SystemLifecycleOperationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Start()
    {
        EnsurePending();
        Status = SystemLifecycleOperationStatus.Running;
        StartedAtUtc = DateTime.UtcNow;
        Touch();
    }

    public void Complete()
    {
        EnsureRunning();
        Status = SystemLifecycleOperationStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
        ErrorMessage = null;
        Touch();
    }

    public void Fail(string? errorMessage = null)
    {
        EnsureRunning();
        Status = SystemLifecycleOperationStatus.Failed;
        CompletedAtUtc = DateTime.UtcNow;
        ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? null : errorMessage.Trim();
        Touch();
    }

    public void Cancel()
    {
        EnsurePending();
        Status = SystemLifecycleOperationStatus.Cancelled;
        CompletedAtUtc = DateTime.UtcNow;
        Touch();
    }

    private void EnsurePending()
    {
        switch (Status)
        {
            case SystemLifecycleOperationStatus.Pending:
                return;

            case SystemLifecycleOperationStatus.Running:
                throw new SystemLifecycleOperationAlreadyRunningException(Id);

            case SystemLifecycleOperationStatus.Completed:
                throw new SystemLifecycleOperationAlreadyCompletedException(Id);

            case SystemLifecycleOperationStatus.Failed:
                throw new SystemLifecycleOperationAlreadyFailedException(Id);

            case SystemLifecycleOperationStatus.Cancelled:
                throw new SystemLifecycleOperationAlreadyCancelledException(Id);

            default:
                throw new SystemLifecycleOperationNotPendingException(Id, Status.ToString());
        }
    }

    private void EnsureRunning()
    {
        switch (Status)
        {
            case SystemLifecycleOperationStatus.Running:
                return;

            case SystemLifecycleOperationStatus.Completed:
                throw new SystemLifecycleOperationAlreadyCompletedException(Id);

            case SystemLifecycleOperationStatus.Failed:
                throw new SystemLifecycleOperationAlreadyFailedException(Id);

            case SystemLifecycleOperationStatus.Cancelled:
                throw new SystemLifecycleOperationAlreadyCancelledException(Id);

            default:
                throw new SystemLifecycleOperationNotRunningException(Id, Status.ToString());
        }
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}