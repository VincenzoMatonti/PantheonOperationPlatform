using Athena.Domain.Common;

namespace Athena.Domain.Block.Registry.Exceptions;

public abstract class BlockRegistryException(
    BlockRegistryErrorCode code,
    string message)
    : DomainException<BlockRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(BlockRegistryErrorCode code)
    {
        return code switch
        {
            BlockRegistryErrorCode.BlockIdRequired
                or BlockRegistryErrorCode.BlockNameRequired
                or BlockRegistryErrorCode.BlockNameTooLong
                or BlockRegistryErrorCode.EcosystemIdRequired
                => DomainErrorType.Validation,

            BlockRegistryErrorCode.BlockNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum BlockRegistryErrorCode
{
    BlockIdRequired = 1,
    BlockNameRequired = 2,
    BlockNameTooLong = 3,
    BlockNameAlreadyExists = 4,

    EcosystemIdRequired = 10,

    BlockNotFound = 20,
    BlockAlreadyRegistered = 21,
    BlockAlreadyUnregistered = 22,
    BlockNotRegistered = 23,
    BlockAlreadyEnabled = 24,
    BlockAlreadyDisabled = 25,
    BlockDeletedCannotBeModified = 26,

    BlockAlreadyAvailable = 30,
    BlockAlreadyUnavailable = 31,
    BlockAlreadyAvailabilityUnknown = 32,
    BlockAlreadyInError = 33
}