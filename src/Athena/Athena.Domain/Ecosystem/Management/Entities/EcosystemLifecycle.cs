using Athena.Domain.Ecosystem.Management.Enums;
using Athena.Domain.Ecosystem.Management.Exceptions;
using Athena.Domain.Common;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Ecosystem.Management.Entities;

public sealed class EcosystemLifecycle
{
    public EcosystemId Id { get; private set; } = null!;
    public EcosystemDesiredState DesiredState { get; private set; }
    public EcosystemOperationalStatus OperationalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static EcosystemLifecycle Create(EcosystemId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return new EcosystemLifecycle { Id = id, CreatedAtUtc = DateTime.UtcNow };
    }
    public void SetDesiredState(EcosystemDesiredState state)
    {
        EnsureActive();
        if (!Enum.IsDefined(state)) throw new EcosystemManagementException(EcosystemManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.", DomainErrorType.Validation);
        if (DesiredState == state) return;
        DesiredState = state;
        Touch();
    }
    public void SetOperationalStatus(EcosystemOperationalStatus status)
    {
        EnsureActive();
        if (!Enum.IsDefined(status)) throw new EcosystemManagementException(EcosystemManagementErrorCode.InvalidOperationalStatus, $"Operational status '{status}' is invalid.", DomainErrorType.Validation);
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
        if (IsDeleted) throw new EcosystemManagementException(EcosystemManagementErrorCode.LifecycleDeletedCannotBeModified, $"Ecosystem lifecycle '{Id.Value}' is deleted.");
    }
    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
