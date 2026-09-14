namespace Hermes.Application.Operations.Exceptions;

public class OperationTypeNotFoundException(Guid operationTypeId) : OperationTypeException(
        OperationTypeErrorCode.NotFound, $"Operation type with ID '{operationTypeId}' was not found.")
{
}

public class OperationTypeNotFoundByCodeException(string code) : OperationTypeException(
        OperationTypeErrorCode.NotFound, $"Operation type with code '{code}' was not found.")
{
}

public class OperationTypeAlreadyExistsException(string code) : OperationTypeException(
        OperationTypeErrorCode.AlreadyExists, $"Operation type with code '{code}' already exists.")
{
}