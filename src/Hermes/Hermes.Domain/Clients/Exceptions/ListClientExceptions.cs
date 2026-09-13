namespace Hermes.Domain.Clients.Exceptions;

public class ClientCodeRequiredException() : ClientException(
        ClientErrorCode.CodeRequired, "Client code cannot be empty.")
{
}

public class ClientNameRequiredException() : ClientException(
        ClientErrorCode.NameRequired, "Client name cannot be empty.")
{
}

public class ClientAlreadyActiveException(Guid clientId) : ClientException(
        ClientErrorCode.AlreadyActive, $"Client with ID '{clientId}' is already active.")
{
}

public class ClientAlreadyInactiveException(Guid clientId) : ClientException(
        ClientErrorCode.AlreadyInactive, $"Client with ID '{clientId}' is already inactive.")
{
}

public class ClientAlreadyDeletedException(Guid clientId) : ClientException(
        ClientErrorCode.AlreadyDeleted, $"Client with ID '{clientId}' is already deleted.")
{
}

public class ClientNotDeletedException(Guid clientId) : ClientException(
        ClientErrorCode.NotDeleted, $"Client with ID '{clientId}' is not deleted.")
{
}