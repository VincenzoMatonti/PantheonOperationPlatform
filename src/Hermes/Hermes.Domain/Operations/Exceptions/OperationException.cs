using Hermes.Domain.Common;

namespace Hermes.Domain.Operations.Exceptions;

public enum OperationErrorCode
{
    ExecutionIdRequired = 1,
    CorrelationIdRequired = 2,
    ExternalIdRequired = 3,
    InvalidValidationState = 4,
    InvalidAcceptanceState = 5,
    InvalidRejectionState = 6
}

public abstract class OperationException(
    OperationErrorCode code,
    string message)
    : DomainException<OperationErrorCode>(code, message)
{
}