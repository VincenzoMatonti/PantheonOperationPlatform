namespace Athena.Domain.System.Registry.Exceptions;

public class SystemInstanceIdRequiredException()
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceIdRequired,
        "System instance ID cannot be empty.")
{
}

public class SystemInstanceNameRequiredException()
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNameRequired,
        "System instance name cannot be empty.")
{
}

public class SystemInstanceNameTooLongException(int maxLength)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNameTooLong,
        $"System instance name cannot exceed {maxLength} characters.")
{
}

public class SystemInstanceNameAlreadyExistsException(
    string instanceName,
    Guid workloadId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNameAlreadyExists,
        $"System instance '{instanceName}' already exists in workload '{workloadId}'.")
{
}

public class SystemInstanceSystemIdRequiredException()
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemIdRequired,
        "System ID cannot be empty.")
{
}

public class SystemInstanceWorkloadIdRequiredException()
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.WorkloadIdRequired,
        "Workload ID cannot be empty.")
{
}

public class SystemInstanceNotFoundException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNotFound,
        $"System instance with ID '{instanceId}' was not found.")
{
}

public class SystemInstanceAlreadyRegisteredException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyRegistered,
        $"System instance with ID '{instanceId}' is already registered.")
{
}

public class SystemInstanceAlreadyUnregisteredException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyUnregistered,
        $"System instance with ID '{instanceId}' is already unregistered.")
{
}

public class SystemInstanceNotRegisteredException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNotRegistered,
        $"System instance with ID '{instanceId}' is not registered.")
{
}

public class SystemInstanceAlreadyEnabledException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyEnabled,
        $"System instance with ID '{instanceId}' is already enabled.")
{
}

public class SystemInstanceAlreadyDisabledException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyDisabled,
        $"System instance with ID '{instanceId}' is already disabled.")
{
}

public class SystemInstanceDisabledCannotBeModifiedException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceDisabledCannotBeModified,
        $"System instance with ID '{instanceId}' is disabled and cannot be modified.")
{
}

public class SystemInstanceDeletedCannotBeModifiedException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceDeletedCannotBeModified,
        $"System instance with ID '{instanceId}' has been deleted and cannot be modified.")
{
}

public class SystemInstanceAlreadyAvailableException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyAvailable,
        $"System instance with ID '{instanceId}' is already available.")
{
}

public class SystemInstanceAlreadyUnavailableException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyUnavailable,
        $"System instance with ID '{instanceId}' is already unavailable.")
{
}

public class SystemInstanceAlreadyAvailabilityUnknownException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyAvailabilityUnknown,
        $"Availability status of system instance with ID '{instanceId}' is already unknown.")
{
}

public class SystemInstanceAlreadyInErrorException(Guid instanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceAlreadyInError,
        $"System instance with ID '{instanceId}' is already in error.")
{
}