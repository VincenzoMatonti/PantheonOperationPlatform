namespace Hermes.Domain.Endpoints.Exceptions;

public class EndpointCodeRequiredException() : EndpointException(
    EndpointErrorCode.CodeRequired, "Endpoint code cannot be empty.")
{
}

public class EndpointTypeRequiredException() : EndpointException(
    EndpointErrorCode.TypeRequired, "Endpoint type cannot be empty.")
{
}

public class EndpointAlreadyActiveException(Guid endpointId) : EndpointException(
    EndpointErrorCode.AlreadyActive, $"Endpoint with ID '{endpointId}' is already active.")
{
}

public class EndpointAlreadyInactiveException(Guid endpointId) : EndpointException(
    EndpointErrorCode.AlreadyInactive, $"Endpoint with ID '{endpointId}' is already inactive.")
{
}

public class EndpointAlreadyDeletedException(Guid endpointId) : EndpointException(
    EndpointErrorCode.AlreadyDeleted, $"Endpoint with ID '{endpointId}' is already deleted.")
{
}

public class EndpointNotDeletedException(Guid endpointId) : EndpointException(
    EndpointErrorCode.NotDeleted, $"Endpoint with ID '{endpointId}' is not deleted.")
{
}