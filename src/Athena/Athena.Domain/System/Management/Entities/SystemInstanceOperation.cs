using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Management.Entities;

public sealed class SystemInstanceOperation
{
    public Guid Id { get; private set; }
    public SystemInstanceId SystemInstanceId { get; private set; } = null!;
    public SystemInstanceOperationType Type { get; private set; }
    public SystemInstanceOperationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTime? AgentAcceptedAtUtc { get; private set; }
    public DateTime? AgentCompletedAtUtc { get; private set; }
    public DateTime? CommandDispatchedAtUtc { get; private set; }
    public string? LeaseOwner { get; private set; }
    public DateTime? LeaseExpiresAtUtc { get; private set; }
    public DateTime? NextAttemptAtUtc { get; private set; }
    public DateTime? LastAttemptAtUtc { get; private set; }
    public int AttemptCount { get; private set; }
    public long Version { get; private set; }
    public bool IsActive => Status is SystemInstanceOperationStatus.Pending
        or SystemInstanceOperationStatus.Running
        or SystemInstanceOperationStatus.OutcomeUnknown;

    public static SystemInstanceOperation Create(SystemInstanceId id, SystemInstanceOperationType type)
    {
        ArgumentNullException.ThrowIfNull(id);
        if (!Enum.IsDefined(type)) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"Operation type '{type}' is invalid.", DomainErrorType.Validation);
        return new SystemInstanceOperation
        {
            Id = Guid.NewGuid(),
            SystemInstanceId = id,
            Type = type,
            Status = SystemInstanceOperationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Start()
    {
        EnsureStatus(SystemInstanceOperationStatus.Pending);
        Status = SystemInstanceOperationStatus.Running;
        StartedAtUtc = NextTimestamp(CreatedAtUtc);
        LastAttemptAtUtc = StartedAtUtc;
        Version++;
    }

    public void MarkCommandDispatched(DateTime dispatchedAtUtc)
    {
        EnsureStatus(SystemInstanceOperationStatus.Running);
        if (dispatchedAtUtc.Kind != DateTimeKind.Utc)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, "Command dispatch timestamp must be UTC.", DomainErrorType.Validation);
        CommandDispatchedAtUtc ??= dispatchedAtUtc;
        Version++;
    }

    public void MarkAccepted(DateTime acceptedAtUtc)
    {
        EnsureRecoverableStatus();
        if (acceptedAtUtc.Kind != DateTimeKind.Utc)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, "Agent acceptance timestamp must be UTC.", DomainErrorType.Validation);
        AgentAcceptedAtUtc = acceptedAtUtc;
        Version++;
    }

    public void Complete(DateTime completedAtUtc)
    {
        EnsureRecoverableStatus();
        if (completedAtUtc.Kind != DateTimeKind.Utc)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, "Agent completion timestamp must be UTC.", DomainErrorType.Validation);
        Status = SystemInstanceOperationStatus.Completed;
        AgentCompletedAtUtc = completedAtUtc;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        ClearLease();
        Version++;
    }

    public void Complete() => Complete(DateTime.UtcNow);

    public void Fail(string errorMessage)
    {
        EnsureRecoverableStatus();
        if (string.IsNullOrWhiteSpace(errorMessage)) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationErrorMessageRequired, "Operation error message is required.", DomainErrorType.Validation);
        Status = SystemInstanceOperationStatus.Failed;
        ErrorMessage = errorMessage.Length > 4000 ? errorMessage[..4000] : errorMessage;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        ClearLease();
        Version++;
    }

    public void FailBeforeDispatch(string errorMessage)
    {
        EnsureStatus(SystemInstanceOperationStatus.Pending);
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationErrorMessageRequired, "Operation error message is required.", DomainErrorType.Validation);
        Status = SystemInstanceOperationStatus.Failed;
        ErrorMessage = errorMessage.Length > 4000 ? errorMessage[..4000] : errorMessage;
        CompletedAtUtc = DateTime.UtcNow;
        ClearLease();
        Version++;
    }

    public void MarkOutcomeUnknown(string reason)
    {
        EnsureStatus(SystemInstanceOperationStatus.Running);
        if (string.IsNullOrWhiteSpace(reason))
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationErrorMessageRequired, "An uncertainty reason is required.", DomainErrorType.Validation);
        Status = SystemInstanceOperationStatus.OutcomeUnknown;
        ErrorMessage = reason.Length > 4000 ? reason[..4000] : reason;
        Version++;
    }

    public void RecoverRunning()
    {
        EnsureStatus(SystemInstanceOperationStatus.OutcomeUnknown);
        Status = SystemInstanceOperationStatus.Running;
        ErrorMessage = null;
        Version++;
    }

    public void MarkIndeterminate(string reason)
    {
        EnsureStatus(SystemInstanceOperationStatus.OutcomeUnknown);
        if (string.IsNullOrWhiteSpace(reason))
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationErrorMessageRequired, "A resolution reason is required.", DomainErrorType.Validation);
        if (LeaseExpiresAtUtc is { } leaseExpiresAtUtc && leaseExpiresAtUtc > DateTime.UtcNow)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"Operation '{Id}' is still leased and cannot be resolved.");

        Status = SystemInstanceOperationStatus.Indeterminate;
        ErrorMessage = reason.Length > 4000 ? reason[..4000] : reason.Trim();
        CompletedAtUtc = NextTimestamp(StartedAtUtc ?? CreatedAtUtc);
        ClearLease();
        Version++;
    }

    public void Claim(string leaseOwner, DateTime leaseExpiresAtUtc, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(leaseOwner))
            throw new ArgumentException("A lease owner is required.", nameof(leaseOwner));
        if (leaseExpiresAtUtc.Kind != DateTimeKind.Utc || nowUtc.Kind != DateTimeKind.Utc || leaseExpiresAtUtc <= nowUtc)
            throw new ArgumentException("Lease timestamps must be valid UTC timestamps.");
        if (!IsActive) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"Terminal operation '{Id}' cannot be claimed.");
        LeaseOwner = leaseOwner;
        LeaseExpiresAtUtc = leaseExpiresAtUtc;
        LastAttemptAtUtc = nowUtc;
        AttemptCount++;
        Version++;
    }

    public void ReleaseLease(string leaseOwner)
    {
        EnsureLeaseOwner(leaseOwner);
        LeaseOwner = null;
        LeaseExpiresAtUtc = null;
        Version++;
    }

    public void ScheduleRetry(DateTime nextAttemptAtUtc)
    {
        if (!IsActive) throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"Terminal operation '{Id}' cannot be scheduled.");
        if (nextAttemptAtUtc.Kind != DateTimeKind.Utc)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, "Retry timestamp must be UTC.", DomainErrorType.Validation);
        NextAttemptAtUtc = nextAttemptAtUtc;
        Version++;
    }

    public bool IsLeasedBy(string leaseOwner) => LeaseOwner == leaseOwner;

    private void EnsureLeaseOwner(string leaseOwner)
    {
        if (!IsActive || LeaseOwner != leaseOwner)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"Lease for operation '{Id}' is no longer owned by '{leaseOwner}'.");
    }

    private void ClearLease()
    {
        LeaseOwner = null;
        LeaseExpiresAtUtc = null;
        NextAttemptAtUtc = null;
    }

    private static DateTime NextTimestamp(DateTime previous)
    {
        var now = DateTime.UtcNow;
        return now > previous ? now : previous.AddTicks(1);
    }

    private void EnsureStatus(SystemInstanceOperationStatus expected)
    {
        if (Status != expected)
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"SystemInstance operation '{Id}' is '{Status}', expected '{expected}'.");
    }

    private void EnsureRecoverableStatus()
    {
        if (Status is not (SystemInstanceOperationStatus.Running or SystemInstanceOperationStatus.OutcomeUnknown))
            throw new SystemInstanceManagementException(SystemInstanceManagementErrorCode.OperationInvalidTransition, $"Operation '{Id}' is '{Status}', expected Running or OutcomeUnknown.");
    }
}
