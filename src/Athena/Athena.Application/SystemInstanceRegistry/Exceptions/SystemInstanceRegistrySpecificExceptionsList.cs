namespace Athena.Application.SystemInstanceRegistry.Exceptions;

public sealed class SystemInstanceNotFoundException(Guid systemInstanceId)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNotFound,
        $"System instance '{systemInstanceId}' was not found.");

public sealed class SystemInstanceNotFoundByNameException(
    Guid workloadId,
    string name)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNotFoundByName,
        $"System instance '{name}' was not found in workload '{workloadId}'.");

public sealed class SystemInstanceNameAlreadyExistsException(
    Guid workloadId,
    string name)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.SystemInstanceNameAlreadyExists,
        $"System instance name '{name}' already exists in workload '{workloadId}'.");

public sealed class InvalidSystemInstanceRegistryRequestException(string reason)
    : SystemInstanceRegistryException(
        SystemInstanceRegistryErrorCode.InvalidSystemInstanceRegistryRequest,
        $"Invalid system instance registry request: {reason}");