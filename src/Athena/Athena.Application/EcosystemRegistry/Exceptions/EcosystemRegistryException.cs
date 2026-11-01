using Athena.Application.Common.exceptions;

namespace Athena.Application.EcosystemRegistry.Exceptions;

public abstract class EcosystemRegistryException(EcosystemRegistryErrorCode code, string message) : ApplicationException<EcosystemRegistryErrorCode>(code, "EcosystemRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(EcosystemRegistryErrorCode code) => code switch
    {
        EcosystemRegistryErrorCode.EcosystemNotFound or EcosystemRegistryErrorCode.EcosystemNotFoundByName => ApplicationCoreErrorType.NotFound,
        EcosystemRegistryErrorCode.EcosystemNameAlreadyExists => ApplicationCoreErrorType.Conflict,
        _ => ApplicationCoreErrorType.Internal
    };
}

public enum EcosystemRegistryErrorCode
{
    EcosystemNotFound = 1,
    EcosystemNotFoundByName = 2,
    EcosystemNameAlreadyExists = 3
}