namespace Athena.Domain.Block.Registry.Exceptions;

public class BlockIdRequiredException() : BlockRegistryException(
    BlockRegistryErrorCode.BlockIdRequired,
    "Block ID cannot be empty.")
{
}

public class BlockNameRequiredException() : BlockRegistryException(
    BlockRegistryErrorCode.BlockNameRequired,
    "Block name cannot be empty.")
{
}

public class BlockNameTooLongException(int maxLength) : BlockRegistryException(
    BlockRegistryErrorCode.BlockNameTooLong,
    $"Block name cannot exceed {maxLength} characters.")
{
}

public class BlockNameAlreadyExistsException(string blockName) : BlockRegistryException(
    BlockRegistryErrorCode.BlockNameAlreadyExists,
    $"Block '{blockName}' already exists.")
{
}

public class BlockEcosystemIdRequiredException() : BlockRegistryException(
    BlockRegistryErrorCode.EcosystemIdRequired,
    "Block must belong to an ecosystem.")
{
}

public class BlockNotFoundException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockNotFound,
    $"Block with ID '{blockId}' was not found.")
{
}

public class BlockAlreadyRegisteredException(string blockName) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyRegistered,
    $"Block '{blockName}' is already registered.")
{
}

public class BlockAlreadyUnregisteredException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyUnregistered,
    $"Block with ID '{blockId}' is already unregistered.")
{
}

public class BlockNotRegisteredException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockNotRegistered,
    $"Block with ID '{blockId}' is not registered.")
{
}

public class BlockAlreadyEnabledException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyEnabled,
    $"Block with ID '{blockId}' is already enabled.")
{
}

public class BlockAlreadyDisabledException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyDisabled,
    $"Block with ID '{blockId}' is already disabled.")
{
}

public class BlockDeletedCannotBeModifiedException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockDeletedCannotBeModified,
    $"Block with ID '{blockId}' has been deleted and cannot be modified.")
{
}

public class BlockAlreadyAvailableException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyAvailable,
    $"Block with ID '{blockId}' is already available.")
{
}

public class BlockAlreadyUnavailableException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyUnavailable,
    $"Block with ID '{blockId}' is already unavailable.")
{
}

public class BlockAlreadyAvailabilityUnknownException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyAvailabilityUnknown,
    $"Availability status of block with ID '{blockId}' is already unknown.")
{
}

public class BlockAlreadyInErrorException(Guid blockId) : BlockRegistryException(
    BlockRegistryErrorCode.BlockAlreadyInError,
    $"Block with ID '{blockId}' is already in error.")
{
}