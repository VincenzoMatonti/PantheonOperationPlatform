using Hermes.Application.Common;

namespace Hermes.Application.Clients.Exceptions;

public enum ClientOperationErrorCode
{
    NotFound = 1,
    AlreadyExists = 2
}

public abstract class ClientOperationException(
    ClientOperationErrorCode code,
    string message)
    : ApplicationException<ClientOperationErrorCode>(code, message)
{
}