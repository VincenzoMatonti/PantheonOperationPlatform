namespace Athena.Application.SystemInstanceManagement.Exceptions;

public sealed class SystemInstanceNotFoundException(Guid systemInstanceId)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.SystemInstanceNotFound,
        $"System instance with ID '{systemInstanceId}' was not found.");

public sealed class SystemInstanceLifecycleNotFoundException(Guid systemInstanceId)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.SystemInstanceLifecycleNotFound,
        $"Lifecycle for system instance with ID '{systemInstanceId}' was not found.");

public sealed class SystemInstanceOperationNotFoundException(Guid operationId)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.SystemInstanceOperationNotFound,
        $"System instance operation with ID '{operationId}' was not found.");

public sealed class SystemInstanceOperationAlreadyActiveException(Guid systemInstanceId)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.SystemInstanceOperationAlreadyActive,
        $"System instance '{systemInstanceId}' already has an active operation.");

public sealed class InvalidSystemInstanceRequestException(string reason)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.InvalidSystemInstanceRequest,
        $"Invalid system instance request: {reason}");

public sealed class InvalidSystemInstanceOperationException(string reason)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.InvalidSystemInstanceOperation,
        $"Invalid system instance operation: {reason}");

public sealed class InvalidSystemInstanceLogTailException(int tail)
    : SystemInstanceManagementException(
        SystemInstanceManagementErrorCode.InvalidSystemInstanceLogTail,
        $"Invalid log tail '{tail}'. The requested log tail must be greater than zero.");
