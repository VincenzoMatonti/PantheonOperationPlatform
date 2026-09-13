namespace Hermes.Domain.Clients.Exceptions;

public class ClientOperationClientIdRequiredException() : ClientOperationException(
        ClientOperationErrorCode.ClientIdRequired, "Client ID cannot be empty.")
{
}

public class ClientOperationTypeIdRequiredException() : ClientOperationException(
        ClientOperationErrorCode.OperationTypeIdRequired, "Operation type ID cannot be empty.")
{
}

public class ClientOperationAlreadyEnabledException(Guid clientOperationId) : ClientOperationException(
        ClientOperationErrorCode.AlreadyEnabled, $"Client operation with ID '{clientOperationId}' is already enabled.")
{
}

public class ClientOperationAlreadyDisabledException(Guid clientOperationId) : ClientOperationException(
        ClientOperationErrorCode.AlreadyDisabled, $"Client operation with ID '{clientOperationId}' is already disabled.")
{
}

public class ClientOperationAlreadyDeletedException(Guid clientOperationId) : ClientOperationException(
        ClientOperationErrorCode.AlreadyDeleted, $"Client operation with ID '{clientOperationId}' is already deleted.")
{
}

public class ClientOperationNotDeletedException(Guid clientOperationId) : ClientOperationException(
        ClientOperationErrorCode.NotDeleted, $"Client operation with ID '{clientOperationId}' is not deleted.")
{
}