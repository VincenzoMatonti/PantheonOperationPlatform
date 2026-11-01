namespace Athena.Application.EcosystemRegistry.Exceptions;

public sealed class EcosystemNotFoundException(Guid ecosystemId) : EcosystemRegistryException(EcosystemRegistryErrorCode.EcosystemNotFound, $"Ecosystem with ID '{ecosystemId}' was not found.");

public sealed class EcosystemNotFoundByNameException(string name) : EcosystemRegistryException(EcosystemRegistryErrorCode.EcosystemNotFoundByName, $"Ecosystem '{name}' was not found.");

public sealed class EcosystemNameAlreadyExistsException(string name) : EcosystemRegistryException(EcosystemRegistryErrorCode.EcosystemNameAlreadyExists, $"Ecosystem '{name}' already exists.");