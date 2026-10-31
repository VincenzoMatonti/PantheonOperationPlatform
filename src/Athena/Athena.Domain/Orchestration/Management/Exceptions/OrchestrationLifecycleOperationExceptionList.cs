
namespace Athena.Domain.Orchestration.Management.Exceptions;

public class OrchestrationLifecycleOperationIdRequiredException()
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.IdRequired,
    "An orchestration lifecycle operation ID is required.");

public class OrchestrationLifecycleOperationOrchestrationIdRequiredException()
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.OrchestrationIdRequired,
    "An orchestration ID is required.");

public class OrchestrationLifecycleOperationErrorMessageRequiredException()
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.ErrorMessageRequired,
    "An error message is required when an orchestration lifecycle operation fails.");

public class OrchestrationLifecycleOperationInvalidTypeException()
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.InvalidType,
    "The specified orchestration lifecycle operation type is invalid.");

public class OrchestrationLifecycleOperationInvalidStatusException()
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.InvalidStatus,
    "The specified orchestration lifecycle operation status is invalid.");

public class OrchestrationLifecycleOperationAlreadyRunningException(Guid operationId)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.AlreadyRunning,
    $"Orchestration lifecycle operation '{operationId}' is already running.");

public class OrchestrationLifecycleOperationAlreadyCompletedException(Guid operationId)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.AlreadyCompleted,
    $"Orchestration lifecycle operation '{operationId}' has already completed.");

public class OrchestrationLifecycleOperationAlreadyFailedException(Guid operationId)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.AlreadyFailed,
    $"Orchestration lifecycle operation '{operationId}' has already failed.");

public class OrchestrationLifecycleOperationAlreadyCancelledException(Guid operationId)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.AlreadyCancelled,
    $"Orchestration lifecycle operation '{operationId}' has already been cancelled.");

public class OrchestrationLifecycleOperationAlreadyDeletedException(Guid operationId)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.AlreadyDeleted,
    $"Orchestration lifecycle operation '{operationId}' has already been deleted.");

public class OrchestrationLifecycleOperationDeletedCannotBeModifiedException(Guid operationId)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.DeletedCannotBeModified,
    $"Orchestration lifecycle operation '{operationId}' cannot be modified because it has been deleted.");

public class OrchestrationLifecycleOperationCannotStartFromCurrentStatusException(Guid operationId, Enums.OrchestrationLifecycleOperationStatus status)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.CannotStartFromCurrentStatus,
    $"Orchestration lifecycle operation '{operationId}' cannot start from status '{status}'.");

public class OrchestrationLifecycleOperationCannotCompleteFromCurrentStatusException(Guid operationId, Enums.OrchestrationLifecycleOperationStatus status)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.CannotCompleteFromCurrentStatus,
    $"Orchestration lifecycle operation '{operationId}' cannot complete from status '{status}'.");

public class OrchestrationLifecycleOperationCannotFailFromCurrentStatusException(Guid operationId, Enums.OrchestrationLifecycleOperationStatus status)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.CannotFailFromCurrentStatus,
    $"Orchestration lifecycle operation '{operationId}' cannot fail from status '{status}'.");

public class OrchestrationLifecycleOperationCannotCancelFromCurrentStatusException(Guid operationId, Enums.OrchestrationLifecycleOperationStatus status)
    : OrchestrationLifecycleOperationException(OrchestrationLifecycleOperationErrorCode.CannotCancelFromCurrentStatus,
    $"Orchestration lifecycle operation '{operationId}' cannot be cancelled from status '{status}'.");
