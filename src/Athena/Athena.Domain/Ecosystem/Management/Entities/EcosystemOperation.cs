using Athena.Domain.Ecosystem.Management.Enums;
using Athena.Domain.Ecosystem.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Ecosystem.Management.Entities;

public sealed class EcosystemOperation
{
    public Guid Id { get; private set; }
    public EcosystemId EcosystemId { get; private set; } = null!;
    public EcosystemOperationType Type { get; private set; }
    public EcosystemOperationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool IsDeleted { get; private set; }

    public static EcosystemOperation Create(Guid id, EcosystemId ecosystemId, EcosystemOperationType type)
    {
        if (id == Guid.Empty) throw new ArgumentException("Operation ID is required.", nameof(id));
        ArgumentNullException.ThrowIfNull(ecosystemId);
        if (!Enum.IsDefined(type)) throw new EcosystemManagementException(EcosystemManagementErrorCode.OperationInvalidTransition, $"Operation type '{type}' is invalid.", DomainErrorType.Validation);
        return new EcosystemOperation { Id = id, EcosystemId = ecosystemId, Type = type, Status = EcosystemOperationStatus.Pending, CreatedAtUtc = DateTime.UtcNow };
    }
    public void Start()
    {
        EnsureActive();
        EnsurePending();
        Status = EcosystemOperationStatus.Running;
        StartedAtUtc = NextTimestamp(CreatedAtUtc);
        Touch();
    }
    public void Complete()
    {
        EnsureActive();
        if (Status != EcosystemOperationStatus.Running) throw new EcosystemManagementException(EcosystemManagementErrorCode.OperationInvalidTransition, $"Ecosystem operation '{Id}' cannot complete from '{Status}'.");
        Status = EcosystemOperationStatus.Completed;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        Touch();
    }
    public void Fail(string errorMessage)
    {
        EnsureActive();
        if (Status != EcosystemOperationStatus.Running) throw new EcosystemManagementException(EcosystemManagementErrorCode.OperationInvalidTransition, $"Ecosystem operation '{Id}' cannot fail from '{Status}'.");
        if (string.IsNullOrWhiteSpace(errorMessage)) throw new EcosystemManagementException(EcosystemManagementErrorCode.OperationErrorMessageRequired, "Operation error message is required.", DomainErrorType.Validation);
        Status = EcosystemOperationStatus.Failed;
        ErrorMessage = errorMessage.Length > 4000 ? errorMessage[..4000] : errorMessage;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        Touch();
    }
    public void Cancel()
    {
        EnsureActive();
        EnsurePending();
        Status = EcosystemOperationStatus.Cancelled;
        CompletedAtUtc = NextTimestamp(CreatedAtUtc);
        Touch();
    }
    public void Delete()
    {
        EnsureActive();
        IsDeleted = true;
        Touch();
    }
    private void EnsurePending()
    {
        if (Status != EcosystemOperationStatus.Pending) throw new EcosystemManagementException(EcosystemManagementErrorCode.OperationInvalidTransition, $"Ecosystem operation '{Id}' is '{Status}', expected Pending.");
    }
    private void EnsureActive()
    {
        if (IsDeleted) throw new EcosystemManagementException(EcosystemManagementErrorCode.OperationInvalidTransition, $"Ecosystem operation '{Id}' is deleted.");
    }
    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
    private static DateTime NextTimestamp(DateTime previous)
    {
        var now = DateTime.UtcNow;
        return now > previous ? now : previous.AddTicks(1);
    }
}
