namespace Hermes.Application.Operations.Exceptions;

public class OperationNotFoundException(Guid operationId) : OperationException(
        OperationErrorCode.NotFound, $"Operation with ID '{operationId}' was not found.")
{
}

public class OperationAlreadyExistsByExternalIdException(string externalId) : OperationException(
        OperationErrorCode.AlreadyExistsByExternalId, $"Operation with external ID '{externalId}' already exists.")
{
}

public class OperationAlreadyExistsByCorrelationIdException(Guid correlationId) : OperationException(
        OperationErrorCode.AlreadyExistsByCorrelationId, $"Operation with correlation ID '{correlationId}' already exists.")
{
}

public class OperationNotFoundByExternalIdException(string externalId) : OperationException(
        OperationErrorCode.NotFound, $"Operation with external ID '{externalId}' was not found.")
{
}

public class OperationNotFoundByCorrelationIdException(Guid correlationId) : OperationException(
        OperationErrorCode.NotFound, $"Operation with correlation ID '{correlationId}' was not found.")
{
}