using Athena.Domain.Common;

namespace Athena.Domain.Block.Management.Exceptions;

public enum BlockManagementErrorCode
{
    LifecycleDeletedCannotBeModified = 1,
    InvalidDesiredState = 2,
    InvalidOperationalStatus = 3,
    OperationInvalidTransition = 4,
    OperationErrorMessageRequired = 5
}

public sealed class BlockManagementException(
    BlockManagementErrorCode code,
    string message,
    DomainErrorType errorType = DomainErrorType.Conflict)
    : DomainException<BlockManagementErrorCode>(code, message, errorType);
