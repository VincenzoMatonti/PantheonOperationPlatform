using Hermes.Domain.Common;

namespace Hermes.Domain.Operations.Exceptions;

public enum OperationTypeErrorCode
{
    CodeRequired = 1,
    NameRequired = 2,
    AlreadyActive = 3,
    AlreadyInactive = 4,
    AlreadyDeleted = 5,
    NotDeleted = 6
}

public abstract class OperationTypeException(
    OperationTypeErrorCode code,
    string message)
    : DomainException<OperationTypeErrorCode>(code, message)
{
}