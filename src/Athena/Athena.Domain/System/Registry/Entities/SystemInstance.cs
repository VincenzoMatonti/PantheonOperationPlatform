using Athena.Domain.System.Registry.Exceptions;
using Athena.Domain.System.Registry.Enums;
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
            RegistrationStatus = SystemInstanceRegistrationStatus.Unregistered,
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
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Registered)
            throw new SystemInstanceAlreadyRegisteredException(Id.Value);
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Disabled)
            throw new SystemInstanceDisabledCannotBeModifiedException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Registered;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered)
            throw new SystemInstanceAlreadyUnregisteredException(Id.Value);
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Disabled)
            throw new SystemInstanceDisabledCannotBeModifiedException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Unregistered;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Registered)
            throw new SystemInstanceAlreadyEnabledException(Id.Value);
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered)
            throw new SystemInstanceNotRegisteredException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Registered;
        Touch();
    }

    public void Disable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Disabled)
            throw new SystemInstanceAlreadyDisabledException(Id.Value);
        if (RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered)
            throw new SystemInstanceNotRegisteredException(Id.Value);
        RegistrationStatus = SystemInstanceRegistrationStatus.Disabled;
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