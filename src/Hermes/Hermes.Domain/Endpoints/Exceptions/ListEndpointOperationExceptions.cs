namespace Hermes.Domain.Endpoints.Exceptions;

public class EndpointOperationEndpointIdRequiredException() : EndpointOperationException(
    EndpointOperationErrorCode.EndpointIdRequired, "Endpoint ID cannot be empty.")
{
}

public class EndpointOperationOperationTypeIdRequiredException() : EndpointOperationException(
    EndpointOperationErrorCode.OperationTypeIdRequired, "Operation type ID cannot be empty.")
{
}

public class EndpointOperationAlreadyEnabledException(Guid endpointOperationId) : EndpointOperationException(
    EndpointOperationErrorCode.AlreadyEnabled, $"Endpoint operation with ID '{endpointOperationId}' is already enabled.")
{
}

public class EndpointOperationAlreadyDisabledException(Guid endpointOperationId) : EndpointOperationException(
    EndpointOperationErrorCode.AlreadyDisabled, $"Endpoint operation with ID '{endpointOperationId}' is already disabled.")
{
}

public class EndpointOperationAlreadyDeletedException(Guid endpointOperationId) : EndpointOperationException(
    EndpointOperationErrorCode.AlreadyDeleted, $"Endpoint operation with ID '{endpointOperationId}' is already deleted.")
{
}

public class EndpointOperationNotDeletedException(Guid endpointOperationId) : EndpointOperationException(
    EndpointOperationErrorCode.NotDeleted, $"Endpoint operation with ID '{endpointOperationId}' is not deleted.")
{
}