using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.Exceptions;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.Workload.Registry.Entities;

public sealed class Workload
{
    public WorkloadId Id { get; private set; } = null!;

    public BlockId BlockId { get; private set; } = null!;

    public WorkloadName Name { get; private set; } = null!;

    public WorkloadRegistrationStatus RegistrationStatus { get; private set; }

    public WorkloadAvailabilityStatus AvailabilityStatus { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    private Workload()
    {
    }

    public static Workload Create(WorkloadId id, BlockId blockId, WorkloadName name)
    {
        if (id is null) throw new WorkloadIdRequiredException();
        if (blockId is null) throw new WorkloadBlockIdRequiredException();
        if (name is null) throw new WorkloadNameRequiredException();
        return new Workload
        {
            Id = id,
            BlockId = blockId,
            Name = name,
            RegistrationStatus = WorkloadRegistrationStatus.Registered,
            AvailabilityStatus = WorkloadAvailabilityStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Rename(WorkloadName name)
    {
        if (name is null) throw new WorkloadNameRequiredException();
        EnsureRegistered();
        if (Name == name) return;
        Name = name;
        Touch();
    }

    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == WorkloadRegistrationStatus.Registered) throw new WorkloadAlreadyRegisteredException(Name.Value);
        RegistrationStatus = WorkloadRegistrationStatus.Registered;
        AvailabilityStatus = WorkloadAvailabilityStatus.Unknown;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == WorkloadRegistrationStatus.Unregistered) throw new WorkloadAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = WorkloadRegistrationStatus.Unregistered;
        AvailabilityStatus = WorkloadAvailabilityStatus.Unknown;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == WorkloadRegistrationStatus.Disabled) throw new WorkloadNotRegisteredException(Id.Value);
        if (RegistrationStatus == WorkloadRegistrationStatus.Registered) throw new WorkloadAlreadyEnabledException(Id.Value);
        RegistrationStatus = WorkloadRegistrationStatus.Registered;
        AvailabilityStatus = WorkloadAvailabilityStatus.Unknown;
        Touch();
    }

    public void Disable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == WorkloadRegistrationStatus.Disabled) throw new WorkloadAlreadyDisabledException(Id.Value);
        if (RegistrationStatus == WorkloadRegistrationStatus.Unregistered) throw new WorkloadNotRegisteredException(Id.Value);
        RegistrationStatus = WorkloadRegistrationStatus.Disabled;
        AvailabilityStatus = WorkloadAvailabilityStatus.Unknown;
        Touch();
    }

    public void MarkAvailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == WorkloadAvailabilityStatus.Available) throw new WorkloadAlreadyAvailableException(Id.Value);
        AvailabilityStatus = WorkloadAvailabilityStatus.Available;
        Touch();
    }

    public void MarkUnavailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == WorkloadAvailabilityStatus.Unavailable) throw new WorkloadAlreadyUnavailableException(Id.Value);
        AvailabilityStatus = WorkloadAvailabilityStatus.Unavailable;
        Touch();
    }

    public void MarkAvailabilityUnknown()
    {
        EnsureRegistered();
        if (AvailabilityStatus == WorkloadAvailabilityStatus.Unknown) throw new WorkloadAlreadyAvailabilityUnknownException(Id.Value);
        AvailabilityStatus = WorkloadAvailabilityStatus.Unknown;
        Touch();
    }

    public void MarkError()
    {
        EnsureRegistered();
        if (AvailabilityStatus == WorkloadAvailabilityStatus.Error) throw new WorkloadAlreadyInErrorException(Id.Value);
        AvailabilityStatus = WorkloadAvailabilityStatus.Error;
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
        if (RegistrationStatus == WorkloadRegistrationStatus.Unregistered) throw new WorkloadNotRegisteredException(Id.Value);
        if (RegistrationStatus == WorkloadRegistrationStatus.Disabled) throw new WorkloadNotRegisteredException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new WorkloadDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}