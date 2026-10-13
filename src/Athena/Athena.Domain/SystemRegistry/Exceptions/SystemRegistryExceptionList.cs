namespace Athena.Domain.SystemRegistry.Exceptions;

public class SystemIdRequiredException() : SystemRegistryException(
        SystemRegistryErrorCode.SystemIdRequired, "System ID cannot be empty.")
{
}

public class SystemNameRequiredException() : SystemRegistryException(
        SystemRegistryErrorCode.SystemNameRequired, "System name cannot be empty.")
{
}

public class SystemNameTooLongException(int maxLength) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNameTooLong, $"System name cannot exceed {maxLength} characters.")
{
}

public class SystemEndpointRequiredException() : SystemRegistryException(
        SystemRegistryErrorCode.SystemEndpointRequired, "System endpoint name and address are required.")
{
}

public class SystemEndpointInvalidException(string address) : SystemRegistryException(
        SystemRegistryErrorCode.SystemEndpointInvalid, $"System endpoint address '{address}' must be an absolute HTTP or HTTPS URI.")
{
}

public class SystemEndpointAlreadyExistsException(string endpointName) : SystemRegistryException(
        SystemRegistryErrorCode.SystemEndpointAlreadyExists, $"Endpoint '{endpointName}' already exists.")
{
}

public class SystemCapabilityRequiredException() : SystemRegistryException(
        SystemRegistryErrorCode.SystemCapabilityRequired, "System capability name cannot be empty.")
{
}

public class SystemCapabilityAlreadyExistsException(string capabilityName) : SystemRegistryException(
        SystemRegistryErrorCode.SystemCapabilityAlreadyExists, $"Capability '{capabilityName}' already exists.")
{
}

public class SystemAlreadyEnabledException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemAlreadyEnabled, $"System with ID '{systemId}' is already enabled.")
{
}

public class SystemAlreadyDisabledException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemAlreadyDisabled, $"System with ID '{systemId}' is already disabled.")
{
}


public class SystemAlreadyUnregisteredException(Guid systemId) : SystemRegistryException(
                SystemRegistryErrorCode.SystemAlreadyUnregistered, $"System with ID '{systemId}' is already unregistered.")
{
}

public class SystemNotRegisteredException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNotRegistered, $"System with ID '{systemId}' is not registered.")
{
}


public class SystemNotFoundException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNotFound, $"System with ID '{systemId}' was not found.")
{
}

public class SystemAlreadyRegisteredException(string systemName) : SystemRegistryException(
        SystemRegistryErrorCode.SystemAlreadyRegistered, $"System '{systemName}' is already registered.")
{
}

public class SystemNameAlreadyExistsException(string systemName) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNameAlreadyExists, $"System '{systemName}' already exists.")
{
}

public class SystemAlreadyAvailableException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemAlreadyAvailable, $"System with ID '{systemId}' is already available.")
{
}

public class SystemAlreadyUnavailableException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemAlreadyUnavailable, $"System with ID '{systemId}' is already unavailable.")
{
}

public class SystemAlreadyAvailabilityUnknownException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemAlreadyAvailabilityUnknown, $"Availability status of system with ID '{systemId}' is already unknown.")
{
}

public class SystemDisabledCannotBeModifiedException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemDisabledCannotBeModified, $"System with ID '{systemId}' is disabled and cannot be modified.")
{
}