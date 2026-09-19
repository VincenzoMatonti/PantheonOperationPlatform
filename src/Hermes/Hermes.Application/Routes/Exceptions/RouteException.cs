using Hermes.Application.Common;

namespace Hermes.Application.Routes.Exceptions;

public enum RouteErrorCode
{
    NotFound = 1,
    AlreadyExists = 2
}

public abstract class RouteException(
    RouteErrorCode code,
    string message)
    : ApplicationException<RouteErrorCode>(code, message)
{
}

