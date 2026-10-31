
using Athena.Domain.Orchestration.Registry.Enums;
using Athena.Domain.Orchestration.Registry.Exceptions;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Registry.Entities;

public class Orchestration
{
    public OrchestrationId Id { get; private set; } = null!;
    public OrchestrationName Name { get; private set; } = null!;
    public OrchestrationRegistrationStatus RegistrationStatus { get; private set; }
    public OrchestrationAvailabilityStatus AvailabilityStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Orchestration Create(OrchestrationId id, OrchestrationName name)
    {
        if (id == null) throw new OrchestrationIdRequiredException();
        if (name == null) throw new OrchestrationNameRequiredException();
        return new Orchestration
        {
            Id = id,
            Name = name,
            RegistrationStatus = OrchestrationRegistrationStatus.Registered,
            AvailabilityStatus = OrchestrationAvailabilityStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus != OrchestrationRegistrationStatus.Unregistered)
            throw new OrchestrationAlreadyRegisteredException(Name.Value);

        RegistrationStatus = OrchestrationRegistrationStatus.Registered;
        AvailabilityStatus = OrchestrationAvailabilityStatus.Unknown;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == OrchestrationRegistrationStatus.Unregistered)
            throw new OrchestrationAlreadyUnregisteredException(Id.Value);

        RegistrationStatus = OrchestrationRegistrationStatus.Unregistered;
        AvailabilityStatus = OrchestrationAvailabilityStatus.Unknown;
        Touch();
    }

    public void Enable()
    {
        EnsureRegistered();
        if (RegistrationStatus == OrchestrationRegistrationStatus.Registered)
            throw new OrchestrationAlreadyEnabledException(Id.Value);

        RegistrationStatus = OrchestrationRegistrationStatus.Registered;
        Touch();
    }

    public void Disable()
    {
        EnsureRegistered();
        if (RegistrationStatus == OrchestrationRegistrationStatus.Disabled)
            throw new OrchestrationAlreadyDisabledException(Id.Value);

        RegistrationStatus = OrchestrationRegistrationStatus.Disabled;
        Touch();
    }

    public void Rename(OrchestrationName name)
    {
        EnsureRegistered();
        Name = name ?? throw new OrchestrationNameRequiredException();
        Touch();
    }


    public static void EnsureNameIsUnique(OrchestrationName name, IEnumerable<OrchestrationName> existingNames)
    {
        if (name == null) throw new OrchestrationNameRequiredException();
        if (existingNames == null) throw new OrchestrationNamesRequiredException();
        if (existingNames.Any(existing => string.Equals(existing.Value, name.Value, StringComparison.OrdinalIgnoreCase)))
            throw new OrchestrationNameAlreadyExistsException(name.Value);
    }

    public void MarkAvailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == OrchestrationAvailabilityStatus.Available)
            throw new OrchestrationAlreadyAvailableException(Id.Value);

        AvailabilityStatus = OrchestrationAvailabilityStatus.Available;
        Touch();
    }

    public void MarkUnavailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == OrchestrationAvailabilityStatus.Unavailable)
            throw new OrchestrationAlreadyUnavailableException(Id.Value);

        AvailabilityStatus = OrchestrationAvailabilityStatus.Unavailable;
        Touch();
    }

    public void MarkAvailabilityUnknown()
    {
        EnsureRegistered();
        if (AvailabilityStatus == OrchestrationAvailabilityStatus.Unknown)
            throw new OrchestrationAlreadyAvailabilityUnknownException(Id.Value);

        AvailabilityStatus = OrchestrationAvailabilityStatus.Unknown;
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
        if (RegistrationStatus == OrchestrationRegistrationStatus.Unregistered)
            throw new OrchestrationNotRegisteredException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new OrchestrationDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
