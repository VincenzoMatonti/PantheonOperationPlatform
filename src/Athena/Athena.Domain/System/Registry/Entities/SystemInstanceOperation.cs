using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Registry.Entities;

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

    public static SystemInstanceOperation Create(SystemInstanceId systemInstanceId, SystemInstanceOperationType type)
    {
        ArgumentNullException.ThrowIfNull(systemInstanceId);
        if (!Enum.IsDefined(type)) throw new ArgumentOutOfRangeException(nameof(type));
        return new SystemInstanceOperation
        {
            Id = Guid.NewGuid(),
            SystemInstanceId = systemInstanceId,
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
    }

    public void Complete()
    {
        EnsureStatus(SystemInstanceOperationStatus.Running);
        Status = SystemInstanceOperationStatus.Completed;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
    }

    public void Fail(string errorMessage)
    {
        EnsureStatus(SystemInstanceOperationStatus.Running);
        if (string.IsNullOrWhiteSpace(errorMessage)) throw new ArgumentException("Error message is required.", nameof(errorMessage));
        Status = SystemInstanceOperationStatus.Failed;
        ErrorMessage = errorMessage.Length > 4000 ? errorMessage[..4000] : errorMessage;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
    }

    private static DateTime NextTimestamp(DateTime previous)
    {
        var now = DateTime.UtcNow;
        return now > previous ? now : previous.AddTicks(1);
    }

    private void EnsureStatus(SystemInstanceOperationStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException($"SystemInstance operation '{Id}' is '{Status}', expected '{expected}'.");
    }
}
