using Athena.Domain.Company.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.Enums;
using Athena.Domain.Ecosystem.Registry.Exceptions;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Ecosystem.Registry.Entities;

public class Ecosystem
{
    public EcosystemId Id { get; private set; } = null!;
    public CompanyId CompanyId { get; private set; } = null!;
    public EcosystemName Name { get; private set; } = null!;
    public EcosystemRegistrationStatus RegistrationStatus { get; private set; }
    public EcosystemAvailabilityStatus AvailabilityStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Ecosystem Create(EcosystemId id, CompanyId companyId, EcosystemName name)
    {
        if (id == null) throw new EcosystemIdRequiredException();
        if (companyId == null) throw new EcosystemCompanyIdRequiredException();
        if (name == null) throw new EcosystemNameRequiredException();
        return new Ecosystem
        {
            Id = id,
            CompanyId = companyId,
            Name = name,
            RegistrationStatus = EcosystemRegistrationStatus.Registered,
            AvailabilityStatus = EcosystemAvailabilityStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = null,
            IsDeleted = false
        };
    }

    // Identity management
    public void Rename(EcosystemName name)
    {
        EnsureRegistered();
        Name = name ?? throw new EcosystemNameRequiredException();
        Touch();
    }

    // Registration lifecycle
    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus != EcosystemRegistrationStatus.Unregistered) throw new EcosystemAlreadyRegisteredException(Name.Value);
        RegistrationStatus = EcosystemRegistrationStatus.Registered;
        AvailabilityStatus = EcosystemAvailabilityStatus.Unknown;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == EcosystemRegistrationStatus.Unregistered) throw new EcosystemAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = EcosystemRegistrationStatus.Unregistered;
        AvailabilityStatus = EcosystemAvailabilityStatus.Unknown;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == EcosystemRegistrationStatus.Unregistered) throw new EcosystemNotRegisteredException(Id.Value);
        if (RegistrationStatus == EcosystemRegistrationStatus.Registered) throw new EcosystemAlreadyEnabledException(Id.Value);
        RegistrationStatus = EcosystemRegistrationStatus.Registered;
        AvailabilityStatus = EcosystemAvailabilityStatus.Unknown;
        Touch();
    }

    public void Disable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == EcosystemRegistrationStatus.Unregistered) throw new EcosystemNotRegisteredException(Id.Value);
        if (RegistrationStatus == EcosystemRegistrationStatus.Disabled) throw new EcosystemAlreadyDisabledException(Id.Value);
        RegistrationStatus = EcosystemRegistrationStatus.Disabled;
        AvailabilityStatus = EcosystemAvailabilityStatus.Unknown;
        Touch();
    }

    // Availability lifecycle
    public void MarkAvailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == EcosystemAvailabilityStatus.Available) throw new EcosystemAlreadyAvailableException(Id.Value);
        AvailabilityStatus = EcosystemAvailabilityStatus.Available;
        Touch();
    }

    public void MarkUnavailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == EcosystemAvailabilityStatus.Unavailable) throw new EcosystemAlreadyUnavailableException(Id.Value);
        AvailabilityStatus = EcosystemAvailabilityStatus.Unavailable;
        Touch();
    }

    public void MarkAvailabilityUnknown()
    {
        EnsureRegistered();
        if (AvailabilityStatus == EcosystemAvailabilityStatus.Unknown) throw new EcosystemAlreadyAvailabilityUnknownException(Id.Value);
        AvailabilityStatus = EcosystemAvailabilityStatus.Unknown;
        Touch();
    }

    public void MarkError()
    {
        EnsureRegistered();
        if (AvailabilityStatus == EcosystemAvailabilityStatus.Error) throw new EcosystemAlreadyInErrorException(Id.Value);
        AvailabilityStatus = EcosystemAvailabilityStatus.Error;
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
        if (RegistrationStatus == EcosystemRegistrationStatus.Unregistered) throw new EcosystemNotRegisteredException(Id.Value);
        if (RegistrationStatus == EcosystemRegistrationStatus.Disabled) throw new EcosystemAlreadyDisabledException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new EcosystemDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}