using Hermes.Domain.Common;

namespace Hermes.Domain.Operations.Exceptions;

public enum OperationErrorCode
{
    ExecutionIdRequired = 1,
    CorrelationIdRequired = 2,
    ExternalIdRequired = 3,
    InvalidSendState = 4,
    InvalidValidationState = 5,
    InvalidAcceptanceState = 6,
    InvalidRejectionState = 7,
    AlreadyDeleted = 8,
    NotDeleted = 9
}

public abstract class OperationException(
    OperationErrorCode code,
    string message)
    : DomainException<OperationErrorCode>(code, message)
{
}