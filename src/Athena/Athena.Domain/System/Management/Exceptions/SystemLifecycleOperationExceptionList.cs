namespace Athena.Domain.System.Management.Exceptions;

public class SystemLifecycleOperationSystemIdRequiredException() : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.SystemIdRequired, "A system ID is required to create a lifecycle operation.")
{
}

public class SystemLifecycleOperationInvalidTypeException() : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.InvalidOperationType, "The specified lifecycle operation type is invalid.")
{
}

public class SystemLifecycleOperationNotPendingException(Guid operationId, string currentStatus) : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationNotPending, $"Operation '{operationId}' must be Pending, but its current status is '{currentStatus}'.")
{
}

public class SystemLifecycleOperationNotRunningException(Guid operationId, string currentStatus) : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationNotRunning, $"Operation '{operationId}' must be Running, but its current status is '{currentStatus}'.")
{
}

public class SystemLifecycleOperationAlreadyRunningException(Guid operationId) : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationAlreadyRunning, $"Operation '{operationId}' is already running.")
{
}

public class SystemLifecycleOperationAlreadyCompletedException(Guid operationId) : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationAlreadyCompleted, $"Operation '{operationId}' has already completed.")
{
}

public class SystemLifecycleOperationAlreadyFailedException(Guid operationId) : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationAlreadyFailed, $"Operation '{operationId}' has already failed.")
{
}

public class SystemLifecycleOperationAlreadyCancelledException(Guid operationId) : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationAlreadyCancelled, $"Operation '{operationId}' has already been cancelled.")
{
}

public class SystemLifecycleOperationErrorMessageRequiredException()
    : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.ErrorMessageRequired,
        "An error message is required when a lifecycle operation fails.")
{
}

public class SystemLifecycleOperationAlreadyDeletedException(Guid operationId)
    : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationAlreadyDeleted,
        $"Operation '{operationId}' has already been deleted.")
{
}

public class SystemLifecycleOperationDeletedCannotBeModifiedException(Guid operationId)
    : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.DeletedOperationCannotBeModified,
        $"Operation '{operationId}' cannot be modified because it has been deleted.")
{
}

public class SystemLifecycleOperationInProgressException(Guid operationId)
    : SystemLifecycleOperationException(
        SystemLifecycleOperationErrorCode.OperationInProgress,
        $"Operation '{operationId}' cannot be deleted while it is running.")
{
}
