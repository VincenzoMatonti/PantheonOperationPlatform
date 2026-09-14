namespace Hermes.Domain.Operations.Exceptions;

public class OperationExecutionIdRequiredException()
    : OperationException(
        OperationErrorCode.ExecutionIdRequired,
        "Execution ID cannot be empty.")
{
}

public class OperationCorrelationIdRequiredException()
    : OperationException(
        OperationErrorCode.CorrelationIdRequired,
        "Correlation ID cannot be empty.")
{
}

public class OperationExternalIdRequiredException()
    : OperationException(
        OperationErrorCode.ExternalIdRequired,
        "External operation ID cannot be empty.")
{
}

public class OperationInvalidSendStateException(Guid operationId) : OperationException(
        OperationErrorCode.InvalidSendState, $"Operation with ID '{operationId}' cannot be sent in its current state.")
{
}

public class OperationInvalidValidationStateException(Guid operationId) : OperationException(
        OperationErrorCode.InvalidValidationState, $"Operation with ID '{operationId}' cannot be validated in its current state.")
{
}

public class OperationInvalidAcceptanceStateException(Guid operationId) : OperationException(
        OperationErrorCode.InvalidAcceptanceState, $"Operation with ID '{operationId}' cannot be accepted in its current state.")
{
}

public class OperationInvalidRejectionStateException(Guid operationId) : OperationException(
        OperationErrorCode.InvalidRejectionState, $"Operation with ID '{operationId}' cannot be rejected in its current state.")
{
}

public class OperationAlreadyDeletedException(Guid operationId) : OperationException(
        OperationErrorCode.AlreadyDeleted, $"Operation with ID '{operationId}' is already deleted.")
{
}

public class OperationNotDeletedException(Guid operationId) : OperationException(
        OperationErrorCode.NotDeleted, $"Operation with ID '{operationId}' is not deleted.")
{
}