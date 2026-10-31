namespace Athena.Domain.Workload.Registry.Exceptions;

public class WorkloadIdRequiredException() : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadIdRequired,
    "Workload ID cannot be empty.")
{
}

public class WorkloadNameRequiredException() : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadNameRequired,
    "Workload name cannot be empty.")
{
}

public class WorkloadNameTooLongException(int maxLength) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadNameTooLong,
    $"Workload name cannot exceed {maxLength} characters.")
{
}

public class WorkloadNameAlreadyExistsException(string workloadName) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadNameAlreadyExists,
    $"Workload '{workloadName}' already exists in this block.")
{
}

public class WorkloadBlockIdRequiredException() : WorkloadRegistryException(
    WorkloadRegistryErrorCode.BlockIdRequired,
    "Workload must belong to a block.")
{
}

public class WorkloadNotFoundException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadNotFound,
    $"Workload with ID '{workloadId}' was not found.")
{
}

public class WorkloadAlreadyRegisteredException(string workloadName) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyRegistered,
    $"Workload '{workloadName}' is already registered.")
{
}

public class WorkloadAlreadyUnregisteredException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyUnregistered,
    $"Workload with ID '{workloadId}' is already unregistered.")
{
}

public class WorkloadNotRegisteredException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadNotRegistered,
    $"Workload with ID '{workloadId}' is not registered.")
{
}

public class WorkloadAlreadyEnabledException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyEnabled,
    $"Workload with ID '{workloadId}' is already enabled.")
{
}

public class WorkloadAlreadyDisabledException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyDisabled,
    $"Workload with ID '{workloadId}' is already disabled.")
{
}

public class WorkloadDeletedCannotBeModifiedException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadDeletedCannotBeModified,
    $"Workload with ID '{workloadId}' has been deleted and cannot be modified.")
{
}

public class WorkloadAlreadyAvailableException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyAvailable,
    $"Workload with ID '{workloadId}' is already available.")
{
}

public class WorkloadAlreadyUnavailableException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyUnavailable,
    $"Workload with ID '{workloadId}' is already unavailable.")
{
}

public class WorkloadAlreadyAvailabilityUnknownException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyAvailabilityUnknown,
    $"Availability status of workload with ID '{workloadId}' is already unknown.")
{
}

public class WorkloadAlreadyInErrorException(Guid workloadId) : WorkloadRegistryException(
    WorkloadRegistryErrorCode.WorkloadAlreadyInError,
    $"Workload with ID '{workloadId}' is already in error.")
{
}