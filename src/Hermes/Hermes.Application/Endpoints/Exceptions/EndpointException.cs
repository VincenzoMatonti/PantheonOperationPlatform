using Hermes.Application.Common;

namespace Hermes.Application.Endpoints.Exceptions;

public enum EndpointErrorCode
{
    NotFoundById = 1,
    NotFoundByCode = 2,
    NotFoundByType = 3,
    AlreadyExistsByCode = 4,
    AlreadyExistsByType = 5
}

public abstract class EndpointException(
    EndpointErrorCode code,
    string message)
    : ApplicationException<EndpointErrorCode>(code, message)
{
}