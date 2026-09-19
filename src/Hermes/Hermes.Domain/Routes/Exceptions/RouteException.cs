using Hermes.Domain.Common;

namespace Hermes.Domain.Routes.Exceptions;

public enum RouteErrorCode
{
    ClientIdRequired = 1,
    OperationTypeIdRequired = 2,
    EndpointIdRequired = 3,
    AlreadyActive = 4,
    AlreadyInactive = 5,
    AlreadyDeleted = 6,
    NotDeleted = 7
}

public abstract class RouteException(
    RouteErrorCode code,
    string message)
    : DomainException<RouteErrorCode>(code, message)
{
}


