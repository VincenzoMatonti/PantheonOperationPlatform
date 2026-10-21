namespace Athena.Application.OrchestrationRegistry.Exceptions;

public class OrchestrationNotFoundException(Guid orchestrationId) : OrchestrationRegistryException(
        OrchestrationRegistryErrorCode.OrchestrationNotFound, $"Orchestration with ID '{orchestrationId}' was not found.")
{
}

public class OrchestrationNameAlreadyExistsException(string orchestrationName) : OrchestrationRegistryException(
        OrchestrationRegistryErrorCode.OrchestrationNameAlreadyExists, $"Orchestration '{orchestrationName}' already exists.")
{
}

public class OrchestrationNotFoundByNameException(string orchestrationName) : OrchestrationRegistryException(
        OrchestrationRegistryErrorCode.OrchestrationNotFoundByName, $"Orchestration with name '{orchestrationName}' was not found.")
{
}