namespace Hermes.Domain.Routes.Exceptions;

public class RouteClientIdRequiredException() : RouteException(
        RouteErrorCode.ClientIdRequired, "Client ID is required.")
{
}

public class RouteOperationTypeIdRequiredException() : RouteException(
        RouteErrorCode.OperationTypeIdRequired, "Operation type ID is required.")
{
}

public class RouteEndpointIdRequiredException() : RouteException(
        RouteErrorCode.EndpointIdRequired, "Endpoint ID is required.")
{
}

public class RouteAlreadyActiveException() : RouteException(
        RouteErrorCode.AlreadyActive, "Route is already active.")
{
}

public class RouteAlreadyInactiveException() : RouteException(
        RouteErrorCode.AlreadyInactive, "Route is already inactive.")
{
}

public class RouteAlreadyDeletedException() : RouteException(
        RouteErrorCode.AlreadyDeleted, "Route is already deleted.")
{
}

public class RouteNotDeletedException() : RouteException(
        RouteErrorCode.NotDeleted, "Route is not deleted.")
{
}