namespace Hermes.Application.Endpoints.Exceptions;

public class EndpointOperationNotFoundException(Guid endpointOperationId)
    : EndpointOperationException(EndpointOperationErrorCode.NotFoundById,
    $"Endpoint operation with ID '{endpointOperationId}' was not found.")
{
}

public class EndpointOperationNotFoundByEndpointAndOperationTypeException(Guid endpointId, Guid operationTypeId)
    : EndpointOperationException(EndpointOperationErrorCode.NotFoundByEndpointAndOperationType,
    $"Endpoint operation with Endpoint ID '{endpointId}' and Operation Type ID '{operationTypeId}' was not found.")
{
}

public class EndpointOperationAlreadyExistsByEndpointAndOperationTypeException(Guid endpointId, Guid operationTypeId)
    : EndpointOperationException(EndpointOperationErrorCode.AlreadyExistsByEndpointAndOperationType,
    $"Endpoint operation with Endpoint ID '{endpointId}' and Operation Type ID '{operationTypeId}' already exists.")
{
}

