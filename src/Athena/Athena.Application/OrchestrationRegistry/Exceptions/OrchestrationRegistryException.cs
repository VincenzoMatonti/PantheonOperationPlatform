
using Athena.Application.Common.exceptions;

namespace Athena.Application.OrchestrationRegistry.Exceptions;

public abstract class OrchestrationRegistryException(
    OrchestrationRegistryErrorCode code, string message)
    : ApplicationException<OrchestrationRegistryErrorCode>(code, "OrchestrationRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(OrchestrationRegistryErrorCode code)
    {
        return code switch
        {
            OrchestrationRegistryErrorCode.OrchestrationNotFound or OrchestrationRegistryErrorCode.OrchestrationNotFoundByName
                => ApplicationCoreErrorType.NotFound,

            OrchestrationRegistryErrorCode.OrchestrationNameAlreadyExists
                => ApplicationCoreErrorType.Conflict,

            _ => ApplicationCoreErrorType.Internal
        };
    }
}

public enum OrchestrationRegistryErrorCode
{
    OrchestrationNotFound = 1,
    OrchestrationNameAlreadyExists = 2,
    OrchestrationNotFoundByName = 3
}
