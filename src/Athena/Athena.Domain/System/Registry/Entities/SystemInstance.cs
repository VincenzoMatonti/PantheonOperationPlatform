using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Exceptions;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.System.Registry.Entities;

public class SystemInstance
{
    public SystemInstanceId Id { get; private set; } = null!;
    public SystemId SystemId { get; private set; } = null!;
    public WorkloadId WorkloadId { get; private set; } = null!;
    public SystemInstanceName Name { get; private set; } = null!;
    public SystemInstanceRegistrationStatus RegistrationStatus { get; private set; }
    public SystemInstanceAvailabilityStatus AvailabilityStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }


    public static SystemInstance Create(SystemInstanceId id, SystemId systemId, WorkloadId workloadId, SystemInstanceName name)
    {
        if (id is null) throw new SystemInstanceIdRequiredException();
        if (systemId is null) throw new SystemInstanceSystemIdRequiredException();
        if (workloadId is null) throw new SystemInstanceWorkloadIdRequiredException();
        if (name is null) throw new SystemInstanceNameRequiredException();
        return new SystemInstance
        {
            Id = id,
            SystemId = systemId,
            WorkloadId = workloadId,
            Name = name,
            RegistrationStatus = SystemInstanceRegistrationStatus.Registered,
            AvailabilityStatus = SystemInstanceAvailabilityStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Rename(SystemInstanceName name)
    {
        if (name is null) throw new SystemInstanceNameRequiredException();
        EnsureRegistered();
        if (Name == name) return;
        Name = name;
        Touch();
    }

    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Registered) throw new SystemInstanceAlreadyRegisteredException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Registered;
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Unknown;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered) throw new SystemInstanceAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Unregistered;
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Unknown;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Registered) throw new SystemInstanceAlreadyEnabledException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Registered;
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Unknown;
        Touch();
    }

    public void Disable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Disabled) throw new SystemInstanceAlreadyDisabledException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Disabled;
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Unknown;
        Touch();
    }

    public void MarkAvailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemInstanceAvailabilityStatus.Available) throw new SystemInstanceAlreadyAvailableException(Id.Value);
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Available;
        Touch();
    }

    public void MarkUnavailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemInstanceAvailabilityStatus.Unavailable) throw new SystemInstanceAlreadyUnavailableException(Id.Value);
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Unavailable;
        Touch();
    }

    public void MarkAvailabilityUnknown()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemInstanceAvailabilityStatus.Unknown) throw new SystemInstanceAlreadyAvailabilityUnknownException(Id.Value);
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Unknown;
        Touch();
    }

    public void MarkError()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemInstanceAvailabilityStatus.Error) throw new SystemInstanceAlreadyInErrorException(Id.Value);
        AvailabilityStatus = SystemInstanceAvailabilityStatus.Error;
        Touch();
    }

    public void Delete()
    {
        EnsureNotDeleted();
        IsDeleted = true;
        Touch();
    }

    private void EnsureRegistered()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered) throw new SystemInstanceNotRegisteredException(Id.Value);
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Disabled) throw new SystemInstanceDisabledCannotBeModifiedException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new SystemInstanceDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}