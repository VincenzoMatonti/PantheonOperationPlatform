using Hermes.Application.Common;

namespace Hermes.Application.Operations.Exceptions;

public enum OperationTypeErrorCode
{
    NotFound = 1,
    AlreadyExists = 2
}

public abstract class OperationTypeException(
    OperationTypeErrorCode code,
    string message)
    : ApplicationException<OperationTypeErrorCode>(code, message)
{
}