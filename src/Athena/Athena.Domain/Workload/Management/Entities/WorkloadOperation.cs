using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.Workload.Management.Entities;

public sealed class WorkloadOperation
{
    public Guid Id { get; private set; }
    public WorkloadId WorkloadId { get; private set; } = null!;
    public WorkloadOperationType Type { get; private set; }
    public WorkloadOperationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool IsDeleted { get; private set; }

    public static WorkloadOperation Create(Guid id, WorkloadId workloadId, WorkloadOperationType type)
    {
        if (id == Guid.Empty) throw new ArgumentException("Operation ID is required.", nameof(id));
        ArgumentNullException.ThrowIfNull(workloadId);
        if (!Enum.IsDefined(type)) throw new WorkloadManagementException(WorkloadManagementErrorCode.OperationInvalidTransition, $"Operation type '{type}' is invalid.", DomainErrorType.Validation);
        return new WorkloadOperation { Id = id, WorkloadId = workloadId, Type = type, Status = WorkloadOperationStatus.Pending, CreatedAtUtc = DateTime.UtcNow };
    }

    public void Start()
    {
        EnsureNotDeleted();
        EnsurePending();
        Status = WorkloadOperationStatus.Running;
        StartedAtUtc = NextTimestamp(CreatedAtUtc);
        Touch();
    }

    public void Complete()
    {
        EnsureNotDeleted();
        if (Status != WorkloadOperationStatus.Running) throw new WorkloadManagementException(WorkloadManagementErrorCode.OperationInvalidTransition, $"Workload operation '{Id}' cannot complete from '{Status}'.");
        Status = WorkloadOperationStatus.Completed;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        Touch();
    }

    public void Fail(string errorMessage)
    {
        EnsureNotDeleted();
        if (Status != WorkloadOperationStatus.Running) throw new WorkloadManagementException(WorkloadManagementErrorCode.OperationInvalidTransition, $"Workload operation '{Id}' cannot fail from '{Status}'.");
        if (string.IsNullOrWhiteSpace(errorMessage)) throw new WorkloadManagementException(WorkloadManagementErrorCode.OperationErrorMessageRequired, "Operation error message is required.", DomainErrorType.Validation);
        Status = WorkloadOperationStatus.Failed;
        ErrorMessage = errorMessage.Length > 4000 ? errorMessage[..4000] : errorMessage;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        Touch();
    }

    public void Cancel()
    {
        EnsureNotDeleted();
        EnsurePending();
        Status = WorkloadOperationStatus.Cancelled;
        CompletedAtUtc = NextTimestamp(CreatedAtUtc);
        Touch();
    }

    public void Delete()
    {
        EnsureNotDeleted();
        IsDeleted = true;
        Touch();
    }

    private void EnsurePending()
    {
        if (Status != WorkloadOperationStatus.Pending) throw new WorkloadManagementException(WorkloadManagementErrorCode.OperationInvalidTransition, $"Workload operation '{Id}' is '{Status}', expected Pending.");
    }
    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new WorkloadManagementException(WorkloadManagementErrorCode.OperationInvalidTransition, $"Workload operation '{Id}' is deleted.");
    }
    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
    private static DateTime NextTimestamp(DateTime previous)
    {
        var now = DateTime.UtcNow;
        return now > previous ? now : previous.AddTicks(1);
    }
}
