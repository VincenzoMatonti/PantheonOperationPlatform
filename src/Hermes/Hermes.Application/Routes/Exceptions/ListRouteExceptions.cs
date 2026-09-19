namespace Hermes.Application.Routes.Exceptions;

public class RouteNotFoundException(Guid routeId) : RouteException(
    RouteErrorCode.NotFound, $"Route with ID '{routeId}' was not found.")
{
}

public class RouteNotFoundByClientOperationTypeAndEndpointException
    (Guid clientId, Guid operationTypeId, Guid endpointId) : RouteException(RouteErrorCode.NotFound,
    $"Route with Client ID '{clientId}', Operation type ID '{operationTypeId}' and Endpoint ID '{endpointId}' was not found.")
{
}

public class RouteAlreadyExistsException(
    Guid clientId, Guid operationTypeId, Guid endpointId) : RouteException(RouteErrorCode.AlreadyExists,
         $"Route with Client ID '{clientId}', Operation type ID '{operationTypeId}' and Endpoint ID '{endpointId}' already exists.")
{
}

