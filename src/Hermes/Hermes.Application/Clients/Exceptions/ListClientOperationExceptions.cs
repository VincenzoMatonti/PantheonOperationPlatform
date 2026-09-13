namespace Hermes.Application.Clients.Exceptions;

public class ClientOperationNotFoundException(Guid clientOperationId) : ClientOperationException(
        ClientOperationErrorCode.NotFound, $"Client operation with ID '{clientOperationId}' was not found.")
{
}

public class ClientOperationNotFoundByClientAndOperationTypeException(Guid clientId, Guid operationTypeId) : ClientOperationException(
        ClientOperationErrorCode.NotFound, $"Client operation for client ID '{clientId}' and operation type ID '{operationTypeId}' was not found.")
{
}

public class ClientOperationAlreadyExistsException(Guid clientId, Guid operationTypeId) : ClientOperationException(
        ClientOperationErrorCode.AlreadyExists, $"Client operation for client ID '{clientId}' and operation type ID '{operationTypeId}' already exists.")
{
}