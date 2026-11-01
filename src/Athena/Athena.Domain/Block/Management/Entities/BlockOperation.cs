using Athena.Domain.Block.Management.Enums;
using Athena.Domain.Block.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.Block.Registry.ValueObjects;

namespace Athena.Domain.Block.Management.Entities;

public sealed class BlockOperation
{
    public Guid Id { get; private set; }
    public BlockId BlockId { get; private set; } = null!;
    public BlockOperationType Type { get; private set; }
    public BlockOperationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool IsDeleted { get; private set; }

    public static BlockOperation Create(Guid id, BlockId blockId, BlockOperationType type)
    {
        if (id == Guid.Empty) throw new ArgumentException("Operation ID is required.", nameof(id));
        ArgumentNullException.ThrowIfNull(blockId);
        if (!Enum.IsDefined(type)) throw new BlockManagementException(BlockManagementErrorCode.OperationInvalidTransition, $"Operation type '{type}' is invalid.", DomainErrorType.Validation);
        return new BlockOperation { Id = id, BlockId = blockId, Type = type, Status = BlockOperationStatus.Pending, CreatedAtUtc = DateTime.UtcNow };
    }
    public void Start()
    {
        EnsureActive();
        EnsurePending();
        Status = BlockOperationStatus.Running;
        StartedAtUtc = NextTimestamp(CreatedAtUtc);
        Touch();
    }
    public void Complete()
    {
        EnsureActive();
        if (Status != BlockOperationStatus.Running) throw new BlockManagementException(BlockManagementErrorCode.OperationInvalidTransition, $"Block operation '{Id}' cannot complete from '{Status}'.");
        Status = BlockOperationStatus.Completed;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        Touch();
    }
    public void Fail(string errorMessage)
    {
        EnsureActive();
        if (Status != BlockOperationStatus.Running) throw new BlockManagementException(BlockManagementErrorCode.OperationInvalidTransition, $"Block operation '{Id}' cannot fail from '{Status}'.");
        if (string.IsNullOrWhiteSpace(errorMessage)) throw new BlockManagementException(BlockManagementErrorCode.OperationErrorMessageRequired, "Operation error message is required.", DomainErrorType.Validation);
        Status = BlockOperationStatus.Failed;
        ErrorMessage = errorMessage.Length > 4000 ? errorMessage[..4000] : errorMessage;
        CompletedAtUtc = NextTimestamp(StartedAtUtc!.Value);
        Touch();
    }
    public void Cancel()
    {
        EnsureActive();
        EnsurePending();
        Status = BlockOperationStatus.Cancelled;
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
        if (Status != BlockOperationStatus.Pending) throw new BlockManagementException(BlockManagementErrorCode.OperationInvalidTransition, $"Block operation '{Id}' is '{Status}', expected Pending.");
    }
    private void EnsureActive()
    {
        if (IsDeleted) throw new BlockManagementException(BlockManagementErrorCode.OperationInvalidTransition, $"Block operation '{Id}' is deleted.");
    }
    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
    private static DateTime NextTimestamp(DateTime previous)
    {
        var now = DateTime.UtcNow;
        return now > previous ? now : previous.AddTicks(1);
    }
}
