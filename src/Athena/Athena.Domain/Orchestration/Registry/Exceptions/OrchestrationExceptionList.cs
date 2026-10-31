namespace Athena.Domain.Orchestration.Registry.Exceptions;

public class OrchestrationIdRequiredException() : OrchestrationException(
        OrchestrationErrorCode.OrchestrationIdRequired, "An orchestration ID is required.")
{
}

public class OrchestrationIdInvalidException() : OrchestrationException(
        OrchestrationErrorCode.OrchestrationIdInvalid, "The specified orchestration ID is invalid.")
{
}

public class OrchestrationNameRequiredException() : OrchestrationException(
        OrchestrationErrorCode.OrchestrationNameRequired, "An orchestration name is required.")
{
}

public class OrchestrationAlreadyRegisteredException(string orchestrationName) : OrchestrationException(
        OrchestrationErrorCode.AlreadyRegistered, $"Orchestration '{orchestrationName}' is already registered.")
{
}

public class OrchestrationAlreadyUnregisteredException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyUnregistered, $"Orchestration '{orchestrationId}' is already unregistered.")
{
}

public class OrchestrationAlreadyEnabledException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyEnabled, $"Orchestration '{orchestrationId}' is already enabled.")
{
}

public class OrchestrationAlreadyDisabledException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyDisabled, $"Orchestration '{orchestrationId}' is already disabled.")
{
}

public class OrchestrationAlreadyDeletedException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyDeleted, $"Orchestration '{orchestrationId}' has already been deleted.")
{
}

public class OrchestrationDeletedCannotBeModifiedException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.DeletedOrchestrationCannotBeModified, $"Orchestration '{orchestrationId}' cannot be modified because it has been deleted.")
{
}

public class OrchestrationNotRegisteredException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.NotRegistered, $"Orchestration '{orchestrationId}' is not registered.")
{
}

public class OrchestrationInvalidRegistrationStatusException() : OrchestrationException(
        OrchestrationErrorCode.InvalidRegistrationStatus, "The specified orchestration registration status is invalid.")
{
}

public class OrchestrationNameAlreadyExistsException(string orchestrationName) : OrchestrationException(
        OrchestrationErrorCode.NameAlreadyExists, $"An orchestration named '{orchestrationName}' already exists.")
{
}

public class OrchestrationNamesRequiredException() : OrchestrationException(
        OrchestrationErrorCode.OrchestrationNamesRequired, "The existing orchestration names are required.")
{
}

public class OrchestrationAlreadyAvailableException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyAvailable, $"Orchestration '{orchestrationId}' is already available.")
{
}

public class OrchestrationAlreadyUnavailableException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyUnavailable, $"Orchestration '{orchestrationId}' is already unavailable.")
{
}

public class OrchestrationAlreadyAvailabilityUnknownException(Guid orchestrationId) : OrchestrationException(
        OrchestrationErrorCode.AlreadyAvailabilityUnknown, $"Orchestration '{orchestrationId}' already has an unknown availability status.")
{
}

public class OrchestrationInvalidAvailabilityStatusException() : OrchestrationException(
        OrchestrationErrorCode.InvalidAvailabilityStatus, "The specified orchestration availability status is invalid.")
{
}