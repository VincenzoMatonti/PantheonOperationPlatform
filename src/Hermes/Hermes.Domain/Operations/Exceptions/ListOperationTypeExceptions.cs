namespace Hermes.Domain.Operations.Exceptions;

public class OperationTypeCodeRequiredException() : OperationTypeException(
        OperationTypeErrorCode.CodeRequired, "Operation type code cannot be empty.")
{
}

public class OperationTypeNameRequiredException() : OperationTypeException(
        OperationTypeErrorCode.NameRequired, "Operation type name cannot be empty.")
{
}

public class OperationTypeAlreadyActiveException(Guid operationTypeId) : OperationTypeException(
        OperationTypeErrorCode.AlreadyActive, $"Operation type with ID '{operationTypeId}' is already active.")
{
}

public class OperationTypeAlreadyInactiveException(Guid operationTypeId) : OperationTypeException(
        OperationTypeErrorCode.AlreadyInactive, $"Operation type with ID '{operationTypeId}' is already inactive.")
{
}