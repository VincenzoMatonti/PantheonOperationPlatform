namespace Hermes.Domain.Operations.Exceptions;

public class OperationExecutionIdRequiredException() : OperationException(
        OperationErrorCode.ExecutionIdRequired, "Execution ID cannot be empty.")
{
}

public class OperationCorrelationIdRequiredException() : OperationException(
        OperationErrorCode.CorrelationIdRequired, "Correlation ID cannot be empty.")
{
}

public class OperationExternalIdRequiredException() : OperationException(
        OperationErrorCode.ExternalIdRequired, "External operation ID cannot be empty.")
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