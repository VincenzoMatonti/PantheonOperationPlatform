using Athena.Domain.Common;

namespace Athena.Domain.Orchestration.Management.Exceptions;

public abstract class OrchestrationSystemException(OrchestrationSystemErrorCode code, string message)
    : DomainException<OrchestrationSystemErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(OrchestrationSystemErrorCode code)
    {
        return code switch
        {
            OrchestrationSystemErrorCode.OrchestrationIdRequired or OrchestrationSystemErrorCode.SystemIdRequired => DomainErrorType.Validation,
            _ => DomainErrorType.Conflict
        };
    }
}

public enum OrchestrationSystemErrorCode
{
    OrchestrationIdRequired = 1,
    SystemIdRequired = 2,

    OrchestrationSystemAlreadyDeleted = 10,
    OrchestrationSystemDeletedCannotBeModified = 11,
    OrchestrationSystemAlreadyRestored = 12
}