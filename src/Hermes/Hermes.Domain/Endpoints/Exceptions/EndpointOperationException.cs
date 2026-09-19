using Hermes.Domain.Common;

namespace Hermes.Domain.Endpoints.Exceptions;

public enum EndpointOperationErrorCode
{
    EndpointIdRequired = 1,
    OperationTypeIdRequired = 2,
    AlreadyEnabled = 3,
    AlreadyDisabled = 4,
    AlreadyDeleted = 5,
    NotDeleted = 6
}

public abstract class EndpointOperationException(
    EndpointOperationErrorCode code,
    string message)
    : DomainException<EndpointOperationErrorCode>(code, message)
{
}