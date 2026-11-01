namespace Athena.Application.BlockRegistry.Exceptions;

public sealed class BlockNotFoundException(Guid blockId) : BlockRegistryException(BlockRegistryErrorCode.BlockNotFound, $"Block with ID '{blockId}' was not found.");

public sealed class BlockNotFoundByNameException(string name) : BlockRegistryException(BlockRegistryErrorCode.BlockNotFoundByName, $"Block '{name}' was not found.");

public sealed class BlockNameAlreadyExistsException(string name) : BlockRegistryException(BlockRegistryErrorCode.BlockNameAlreadyExists, $"Block '{name}' already exists.");