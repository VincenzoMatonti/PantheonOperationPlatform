using Athena.Application.Common.exceptions;

namespace Athena.Application.BlockRegistry.Exceptions;

public abstract class BlockRegistryException(BlockRegistryErrorCode code, string message) : ApplicationException<BlockRegistryErrorCode>(code, "BlockRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(BlockRegistryErrorCode code)
    {
        return code switch
        {
            BlockRegistryErrorCode.BlockNotFound or BlockRegistryErrorCode.BlockNotFoundByName => ApplicationCoreErrorType.NotFound,
            BlockRegistryErrorCode.BlockNameAlreadyExists => ApplicationCoreErrorType.Conflict,
            _ => ApplicationCoreErrorType.Internal
        };
    }
}

public enum BlockRegistryErrorCode
{
    BlockNotFound = 1,
    BlockNotFoundByName = 2,
    BlockNameAlreadyExists = 3
}