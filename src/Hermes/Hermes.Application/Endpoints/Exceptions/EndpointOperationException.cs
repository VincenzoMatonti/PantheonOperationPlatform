using Hermes.Application.Common;

namespace Hermes.Application.Endpoints.Exceptions;

public enum EndpointOperationErrorCode
{
    NotFoundById = 1,
    NotFoundByEndpointAndOperationType = 2,
    AlreadyExistsByEndpointAndOperationType = 3
}

public abstract class EndpointOperationException(
    EndpointOperationErrorCode code, string message)
    : ApplicationException<EndpointOperationErrorCode>
    (code, message)
{
}