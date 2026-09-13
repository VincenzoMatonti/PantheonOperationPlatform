namespace Hermes.Application.Clients.Exceptions;

public class ClientNotFoundException(Guid clientId) : ClientException(
        ClientErrorCode.NotFound, $"Client with ID '{clientId}' was not found.")
{
}

public class ClientNotFoundByCodeException(string code) : ClientException(
        ClientErrorCode.NotFound, $"Client with code '{code}' was not found.")
{
}

public class ClientAlreadyExistsException(string code) : ClientException(
        ClientErrorCode.AlreadyExists, $"Client with code '{code}' already exists.")
{
}