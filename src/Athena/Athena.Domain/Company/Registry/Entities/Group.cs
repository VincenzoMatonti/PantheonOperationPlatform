using Athena.Domain.Company.Registry.Enums;
using Athena.Domain.Company.Registry.Exceptions;
using Athena.Domain.Company.Registry.ValueObjects;

namespace Athena.Domain.Company.Registry.Entities;

public class Group
{
    public GroupId Id { get; private set; } = null!;
    public GroupName Name { get; private set; } = null!;
    public GroupRegistrationStatus RegistrationStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Group Create(GroupId id, GroupName name)
    {
        if (id == null) throw new GroupIdRequiredException();
        if (name == null) throw new GroupNameRequiredException();
        return new Group
        {
            Id = id,
            Name = name,
            RegistrationStatus = GroupRegistrationStatus.Registered,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = null,
            IsDeleted = false
        };
    }

    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus != GroupRegistrationStatus.Unregistered) throw new GroupAlreadyRegisteredException(Name.Value);
        RegistrationStatus = GroupRegistrationStatus.Registered;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == GroupRegistrationStatus.Unregistered) throw new GroupAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = GroupRegistrationStatus.Unregistered;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == GroupRegistrationStatus.Unregistered) throw new GroupNotRegisteredException(Id.Value);
        if (RegistrationStatus == GroupRegistrationStatus.Registered) throw new GroupAlreadyEnabledException(Id.Value);
        RegistrationStatus = GroupRegistrationStatus.Registered;
        Touch();
    }

    public void Disable()
    {
        EnsureRegistered();
        if (RegistrationStatus == GroupRegistrationStatus.Disabled) throw new GroupAlreadyDisabledException(Id.Value);
        RegistrationStatus = GroupRegistrationStatus.Disabled;
        Touch();
    }

    public void Rename(GroupName name)
    {
        EnsureRegistered();
        Name = name ?? throw new GroupNameRequiredException();
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
        if (RegistrationStatus == GroupRegistrationStatus.Unregistered) throw new GroupNotRegisteredException(Id.Value);
        if (RegistrationStatus == GroupRegistrationStatus.Disabled) throw new GroupAlreadyDisabledException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new GroupDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}