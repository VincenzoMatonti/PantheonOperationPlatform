namespace Athena.Application.SystemManagement.Exceptions;

public class InvalidDesiredStateException(string desiredState)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.InvalidDesiredState,
        $"Desired state '{desiredState}' is invalid.")
{
    public string DesiredState { get; } = desiredState;
}

public class InvalidOperationalStatusException(string operationalStatus)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.InvalidOperationalStatus,
        $"Operational status '{operationalStatus}' is invalid.")
{
    public string OperationalStatus { get; } = operationalStatus;
}

public class InvalidOperationTypeException(string operationType)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.InvalidOperationType,
        $"Operation type '{operationType}' is invalid.")
{
    public string OperationType { get; } = operationType;
}

public class InvalidOperationStatusException(string operationStatus)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.InvalidOperationStatus,
        $"Operation status '{operationStatus}' is invalid.")
{
    public string OperationStatus { get; } = operationStatus;
}

public class SystemNotFoundException(Guid systemId)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.SystemNotFound,
        $"System '{systemId}' was not found.")
{
    public Guid SystemId { get; } = systemId;
}

public class SystemLifecycleAlreadyExistsException(Guid systemId)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.SystemLifecycleAlreadyExists,
        $"A lifecycle already exists for system '{systemId}'.")
{
    public Guid SystemId { get; } = systemId;
}

public class SystemLifecycleNotFoundException(Guid systemId)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.SystemLifecycleNotFound,
        $"Lifecycle for system '{systemId}' was not found.")
{
    public Guid SystemId { get; } = systemId;
}

public class SystemLifecycleOperationNotFoundException(Guid operationId)
    : SystemManagementApplicationException(
        SystemManagementApplicationErrorCode.SystemLifecycleOperationNotFound,
        $"Lifecycle operation '{operationId}' was not found.")
{
    public Guid OperationId { get; } = operationId;
}