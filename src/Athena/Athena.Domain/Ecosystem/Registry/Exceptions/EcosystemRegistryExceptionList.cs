namespace Athena.Domain.Ecosystem.Registry.Exceptions;

public class EcosystemIdRequiredException() : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemIdRequired,
    "Ecosystem ID cannot be empty.")
{
}

public class EcosystemNameRequiredException() : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemNameRequired,
    "Ecosystem name cannot be empty.")
{
}

public class EcosystemNameTooLongException(int maxLength) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemNameTooLong,
    $"Ecosystem name cannot exceed {maxLength} characters.")
{
}

public class EcosystemNameAlreadyExistsException(string ecosystemName) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemNameAlreadyExists,
    $"Ecosystem '{ecosystemName}' already exists.")
{
}

public class EcosystemCompanyIdRequiredException() : EcosystemRegistryException(
    EcosystemRegistryErrorCode.CompanyIdRequired,
    "Ecosystem must belong to a company.")
{
}

public class EcosystemNotFoundException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemNotFound,
    $"Ecosystem with ID '{ecosystemId}' was not found.")
{
}

public class EcosystemAlreadyRegisteredException(string ecosystemName) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyRegistered,
    $"Ecosystem '{ecosystemName}' is already registered.")
{
}

public class EcosystemAlreadyUnregisteredException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyUnregistered,
    $"Ecosystem with ID '{ecosystemId}' is already unregistered.")
{
}

public class EcosystemNotRegisteredException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemNotRegistered,
    $"Ecosystem with ID '{ecosystemId}' is not registered.")
{
}

public class EcosystemAlreadyEnabledException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyEnabled,
    $"Ecosystem with ID '{ecosystemId}' is already enabled.")
{
}

public class EcosystemAlreadyDisabledException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyDisabled,
    $"Ecosystem with ID '{ecosystemId}' is already disabled.")
{
}

public class EcosystemDeletedCannotBeModifiedException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemDeletedCannotBeModified,
    $"Ecosystem with ID '{ecosystemId}' has been deleted and cannot be modified.")
{
}

public class EcosystemAlreadyAvailableException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyAvailable,
    $"Ecosystem with ID '{ecosystemId}' is already available.")
{
}

public class EcosystemAlreadyUnavailableException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyUnavailable,
    $"Ecosystem with ID '{ecosystemId}' is already unavailable.")
{
}

public class EcosystemAlreadyAvailabilityUnknownException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyAvailabilityUnknown,
    $"Availability status of ecosystem with ID '{ecosystemId}' is already unknown.")
{
}

public class EcosystemAlreadyInErrorException(Guid ecosystemId) : EcosystemRegistryException(
    EcosystemRegistryErrorCode.EcosystemAlreadyInError,
    $"Ecosystem with ID '{ecosystemId}' is already in error.")
{
}