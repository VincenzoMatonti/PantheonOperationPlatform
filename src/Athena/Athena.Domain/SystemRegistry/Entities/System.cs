using Athena.Domain.SystemRegistry.Enums;
using Athena.Domain.SystemRegistry.Exceptions;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.SystemRegistry.Entities;

public class System
{
    private readonly List<SystemEndpoint> _endpoints = [];
    private readonly List<SystemCapability> _capabilities = [];
    public SystemId Id { get; private set; } = null!;
    public SystemName Name { get; private set; } = null!;
    public SystemRegistrationStatus RegistrationStatus { get; private set; }
    public SystemAvailabilityStatus AvailabilityStatus { get; private set; }
    public IReadOnlyCollection<SystemEndpoint> Endpoints => _endpoints.AsReadOnly();
    public IReadOnlyCollection<SystemCapability> Capabilities => _capabilities.AsReadOnly();

    public static System Create(SystemId id, SystemName name)
    {
        if (id == null) throw new SystemIdRequiredException();
        if (name == null) throw new SystemNameRequiredException();
        return new System
        {
            Id = id,
            Name = name,
            RegistrationStatus = SystemRegistrationStatus.Registered,
            AvailabilityStatus = SystemAvailabilityStatus.Unknown
        };
    }

    public void Register()
    {
        if (RegistrationStatus != SystemRegistrationStatus.Unregistered) throw new SystemAlreadyRegisteredException(Name.Value);
        RegistrationStatus = SystemRegistrationStatus.Registered;
        AvailabilityStatus = SystemAvailabilityStatus.Unknown;
    }

    public void Unregister()
    {
        if (RegistrationStatus == SystemRegistrationStatus.Unregistered) throw new SystemAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = SystemRegistrationStatus.Unregistered;
        AvailabilityStatus = SystemAvailabilityStatus.Unknown;
    }

    public void Enable()
    {
        if (RegistrationStatus == SystemRegistrationStatus.Unregistered) throw new SystemNotRegisteredException(Id.Value);
        if (RegistrationStatus == SystemRegistrationStatus.Registered) throw new SystemAlreadyEnabledException(Id.Value);
        RegistrationStatus = SystemRegistrationStatus.Registered;
        AvailabilityStatus = SystemAvailabilityStatus.Unknown;
    }

    public void Disable()
    {
        EnsureRegistered();
        if (RegistrationStatus == SystemRegistrationStatus.Disabled) throw new SystemAlreadyDisabledException(Id.Value);
        RegistrationStatus = SystemRegistrationStatus.Disabled;
        AvailabilityStatus = SystemAvailabilityStatus.Unknown;
    }

    public void Rename(SystemName name)
    {
        EnsureRegistered();
        Name = name ?? throw new SystemNameRequiredException();
    }

    public void AddEndpoint(SystemEndpoint endpoint)
    {
        EnsureRegistered();
        if (endpoint == null) throw new SystemEndpointRequiredException();
        if (_endpoints.Any(existing => existing.Equals(endpoint))) throw new SystemEndpointAlreadyExistsException(endpoint.Name);
        _endpoints.Add(endpoint);
    }

    public void RemoveEndpoint(string endpointName)
    {
        EnsureRegistered();
        if (string.IsNullOrWhiteSpace(endpointName)) throw new SystemEndpointRequiredException();
        var endpoint = _endpoints.FirstOrDefault(existing => string.Equals(existing.Name, endpointName.Trim(), StringComparison.OrdinalIgnoreCase));
        if (endpoint != null) _endpoints.Remove(endpoint);
    }

    public void AddCapability(SystemCapability capability)
    {
        EnsureRegistered();
        if (capability == null) throw new SystemCapabilityRequiredException();
        if (_capabilities.Any(existing => existing.Equals(capability))) throw new SystemCapabilityAlreadyExistsException(capability.Name);
        _capabilities.Add(capability);
    }

    public void RemoveCapability(string capabilityName)
    {
        EnsureRegistered();
        if (string.IsNullOrWhiteSpace(capabilityName)) throw new SystemCapabilityRequiredException();
        var capability = _capabilities.FirstOrDefault(existing => string.Equals(existing.Name, capabilityName.Trim(), StringComparison.OrdinalIgnoreCase));
        if (capability != null) _capabilities.Remove(capability);
    }

    public void MarkAvailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemAvailabilityStatus.Available) throw new SystemAlreadyAvailableException(Id.Value);
        AvailabilityStatus = SystemAvailabilityStatus.Available;
    }

    public void MarkUnavailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemAvailabilityStatus.Unavailable) throw new SystemAlreadyUnavailableException(Id.Value);
        AvailabilityStatus = SystemAvailabilityStatus.Unavailable;
    }

    public void MarkAvailabilityUnknown()
    {
        EnsureRegistered();
        if (AvailabilityStatus == SystemAvailabilityStatus.Unknown) throw new SystemAlreadyAvailabilityUnknownException(Id.Value);
        AvailabilityStatus = SystemAvailabilityStatus.Unknown;
    }

    private void EnsureRegistered()
    {
        if (RegistrationStatus == SystemRegistrationStatus.Unregistered) throw new SystemNotRegisteredException(Id.Value);
        if (RegistrationStatus == SystemRegistrationStatus.Disabled) throw new SystemDisabledCannotBeModifiedException(Id.Value);
    }
}
