using Hermes.Application.Common;

namespace Hermes.Application.Operations.Exceptions;

public enum OperationErrorCode
{
    NotFound = 1,
    AlreadyExistsByExternalId = 2,
    AlreadyExistsByCorrelationId = 3
}

public abstract class OperationException(
    OperationErrorCode code,
    string message)
    : ApplicationException<OperationErrorCode>(code, message)
{
}