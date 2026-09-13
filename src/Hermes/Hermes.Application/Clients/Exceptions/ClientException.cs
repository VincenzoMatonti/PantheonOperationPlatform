using Hermes.Application.Common;

namespace Hermes.Application.Clients.Exceptions;

public enum ClientErrorCode
{
    NotFound = 1,
    AlreadyExists = 2
}

public abstract class ClientException(ClientErrorCode code, string message) : ApplicationException<ClientErrorCode>(code, message)
{
}