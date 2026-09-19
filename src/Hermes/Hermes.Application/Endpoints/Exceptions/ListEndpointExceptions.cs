namespace Hermes.Application.Endpoints.Exceptions;

public class EndpointNotFoundException(Guid endpointId) : EndpointException(
    EndpointErrorCode.NotFoundById, $"Endpoint with ID '{endpointId}' was not found.")
{
}

public class EndpointNotFoundByCodeException(string code) : EndpointException(
    EndpointErrorCode.NotFoundByCode, $"Endpoint with code '{code}' was not found.")
{
}

public class EndpointNotFoundByTypeException(string type) : EndpointException(
    EndpointErrorCode.NotFoundByType, $"Endpoint with type '{type}' was not found.")
{
}

public class EndpointAlreadyExistsByCodeException(string code) : EndpointException(
    EndpointErrorCode.AlreadyExistsByCode, $"Endpoint with code '{code}' already exists.")
{
}

public class EndpointAlreadyExistsByTypeException(string type) : EndpointException(
    EndpointErrorCode.AlreadyExistsByType, $"Endpoint with type '{type}' already exists.")
{
}