namespace Athena.Application.OrchestrationManagement.Exceptions;

public class InvalidDesiredStateException(string desiredState) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.InvalidDesiredState, $"Desired state '{desiredState}' is invalid.")
{
    public string DesiredState { get; } = desiredState;
}

public class InvalidOperationalStatusException(string operationalStatus) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.InvalidOperationalStatus, $"Operational status '{operationalStatus}' is invalid.")
{
    public string OperationalStatus { get; } = operationalStatus;
}

public class InvalidOperationTypeException(string operationType) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.InvalidOperationType, $"Operation type '{operationType}' is invalid.")
{
    public string OperationType { get; } = operationType;
}

public class InvalidOperationStatusException(string operationStatus) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.InvalidOperationStatus, $"Operation status '{operationStatus}' is invalid.")
{
    public string OperationStatus { get; } = operationStatus;
}

public class OrchestrationNotFoundException(Guid orchestrationId) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.OrchestrationNotFound, $"Orchestration '{orchestrationId}' was not found.")
{
    public Guid OrchestrationId { get; } = orchestrationId;
}

public class OrchestrationLifecycleAlreadyExistsException(Guid orchestrationId) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleAlreadyExists, $"A lifecycle already exists for orchestration '{orchestrationId}'.")
{
    public Guid OrchestrationId { get; } = orchestrationId;
}

public class OrchestrationLifecycleNotFoundException(Guid orchestrationId) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleNotFound, $"Lifecycle for orchestration '{orchestrationId}' was not found.")
{
    public Guid OrchestrationId { get; } = orchestrationId;
}

public class OrchestrationLifecycleOperationNotFoundException(Guid operationId) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleOperationNotFound, $"Lifecycle operation '{operationId}' was not found.")
{
    public Guid OperationId { get; } = operationId;
}

public class OrchestrationLifecycleOperationAlreadyExistsException(Guid operationId) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleOperationAlreadyExists, $"Lifecycle operation '{operationId}' already exists.")
{
    public Guid OperationId { get; } = operationId;
}

public class OrchestrationSystemInstanceNotFoundException(Guid id) : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.OrchestrationSystemInstanceNotFound, $"Orchestration-system association '{id}' was not found.")
{
    public Guid Id { get; } = id;
}

public class OrchestrationSystemInstanceAlreadyExistsException(Guid orchestrationId, Guid systemId)
    : OrchestrationManagementApplicationException(OrchestrationManagementApplicationErrorCode.OrchestrationSystemInstanceAlreadyExists,
         $"An association between orchestration '{orchestrationId}' and system '{systemId}' already exists.")
{
    public Guid OrchestrationId { get; } = orchestrationId;

    public Guid SystemInstanceId { get; } = systemId;
}

public sealed class OrchestrationSystemInstanceOperationsFailedException(
    Guid orchestrationId,
    IReadOnlyDictionary<Guid, Exception> failures)
    : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.SystemInstanceOperationsFailed,
        $"Orchestration '{orchestrationId}' had failures while coordinating resources: {string.Join("; ", failures.Select(failure => $"{failure.Key}: {failure.Value.Message}"))}")
{
    public IReadOnlyDictionary<Guid, Exception> Failures { get; } = failures;
}
