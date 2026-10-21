namespace Athena.Application.SystemRegistry.Exceptions;

public class SystemNotFoundException(Guid systemId) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNotFound, $"System with ID '{systemId}' was not found.")
{
}

public class SystemNameAlreadyExistsException(string systemName) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNameAlreadyExists, $"System '{systemName}' already exists.")
{
}

public class SystemNotFoundByNameException(string systemName) : SystemRegistryException(
        SystemRegistryErrorCode.SystemNotFoundByName, $"System with name '{systemName}' was not found.")
{
}