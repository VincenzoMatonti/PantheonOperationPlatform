using Athena.Domain.Block.Management.Enums;
using Athena.Domain.Block.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.Block.Registry.ValueObjects;

namespace Athena.Domain.Block.Management.Entities;

public sealed class BlockLifecycle
{
    public BlockId Id { get; private set; } = null!;
    public BlockDesiredState DesiredState { get; private set; }
    public BlockOperationalStatus OperationalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static BlockLifecycle Create(BlockId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return new BlockLifecycle { Id = id, CreatedAtUtc = DateTime.UtcNow };
    }
    public void SetDesiredState(BlockDesiredState state)
    {
        EnsureActive();
        if (!Enum.IsDefined(state)) throw new BlockManagementException(BlockManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.", DomainErrorType.Validation);
        if (DesiredState == state) return;
        DesiredState = state;
        Touch();
    }
    public void SetOperationalStatus(BlockOperationalStatus status)
    {
        EnsureActive();
        if (!Enum.IsDefined(status)) throw new BlockManagementException(BlockManagementErrorCode.InvalidOperationalStatus, $"Operational status '{status}' is invalid.", DomainErrorType.Validation);
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
        if (IsDeleted) throw new BlockManagementException(BlockManagementErrorCode.LifecycleDeletedCannotBeModified, $"Block lifecycle '{Id.Value}' is deleted.");
    }
    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
