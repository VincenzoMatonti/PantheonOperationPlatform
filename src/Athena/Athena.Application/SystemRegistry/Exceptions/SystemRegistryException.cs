using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemRegistry.Exceptions;

public abstract class SystemRegistryException(
    SystemRegistryErrorCode code,
    string message)
    : ApplicationException<SystemRegistryErrorCode>(code, message)
{
}

public enum SystemRegistryErrorCode
{
    SystemNotFound = 1,
    SystemNameAlreadyExists = 2,
    SystemNotFoundByName = 3
}