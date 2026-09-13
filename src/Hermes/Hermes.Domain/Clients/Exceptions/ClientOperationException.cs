using Hermes.Domain.Common;

namespace Hermes.Domain.Clients.Exceptions;

public enum ClientOperationErrorCode
{
    ClientIdRequired = 1,
    OperationTypeIdRequired = 2,
    AlreadyEnabled = 3,
    AlreadyDisabled = 4,
    AlreadyDeleted = 5,
    NotDeleted = 6
}

public abstract class ClientOperationException(
    ClientOperationErrorCode code,
    string message)
    : DomainException<ClientOperationErrorCode>(code, message)
{
}