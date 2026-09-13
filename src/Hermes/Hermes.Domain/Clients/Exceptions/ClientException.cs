using Hermes.Domain.Common;

namespace Hermes.Domain.Clients.Exceptions;

public enum ClientErrorCode
{
    CodeRequired = 1,
    NameRequired = 2,
    AlreadyActive = 3,
    AlreadyInactive = 4,
    AlreadyDeleted = 5,
    NotDeleted = 6
}

public abstract class ClientException(
    ClientErrorCode code,
    string message)
    : DomainException<ClientErrorCode>(code, message)
{
}