using Hermes.Domain.Common;

namespace Hermes.Domain.Endpoints.Exceptions;

public enum EndpointErrorCode
{
    CodeRequired = 1,
    TypeRequired = 2,
    AlreadyActive = 3,
    AlreadyInactive = 4,
    AlreadyDeleted = 5,
    NotDeleted = 6
}

public abstract class EndpointException(
    EndpointErrorCode code,
    string message)
    : DomainException<EndpointErrorCode>(code, message)
{
}