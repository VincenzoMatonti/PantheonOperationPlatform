using Athena.Api.SystemManagement.Requests;
using Athena.Application.SystemManagement.Commands;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemManagementRequestCommandMapper
{
    public static CreateSystemLifecycleCommand ToCreateLifecycleCommand(Guid systemId) => new()
    {
        SystemId = systemId
    };

    public static SetSystemDesiredStateCommand ToSetDesiredStateCommand(Guid systemId, SetSystemDesiredStateRequest request) => new()
    {
        SystemId = systemId,
        DesiredState = request.DesiredState
    };

    public static MarkSystemStartingCommand ToMarkStartingCommand(Guid systemId) => new() { SystemId = systemId };

    public static MarkSystemRunningCommand ToMarkRunningCommand(Guid systemId) => new() { SystemId = systemId };

    public static MarkSystemStoppingCommand ToMarkStoppingCommand(Guid systemId) => new() { SystemId = systemId };

    public static MarkSystemStoppedCommand ToMarkStoppedCommand(Guid systemId) => new() { SystemId = systemId };

    public static MarkSystemFailedCommand ToMarkFailedCommand(Guid systemId) => new() { SystemId = systemId };

    public static MarkSystemUnknownCommand ToMarkUnknownCommand(Guid systemId) => new() { SystemId = systemId };

    public static DeleteSystemLifecycleCommand ToDeleteLifecycleCommand(Guid systemId) => new() { SystemId = systemId };

    public static CreateSystemLifecycleOperationCommand ToCreateOperationCommand(Guid systemId, CreateSystemLifecycleOperationRequest request) => new()
    {
        SystemId = systemId,
        Type = request.Type
    };

    public static StartSystemLifecycleOperationCommand ToStartOperationCommand(Guid operationId) => new() { OperationId = operationId };

    public static CompleteSystemLifecycleOperationCommand ToCompleteOperationCommand(Guid operationId) => new() { OperationId = operationId };

    public static FailSystemLifecycleOperationCommand ToFailOperationCommand(Guid operationId, FailSystemLifecycleOperationRequest request) => new()
    {
        OperationId = operationId,
        ErrorMessage = request.ErrorMessage
    };

    public static CancelSystemLifecycleOperationCommand ToCancelOperationCommand(Guid operationId) => new() { OperationId = operationId };

    public static DeleteSystemLifecycleOperationCommand ToDeleteOperationCommand(Guid operationId) => new() { OperationId = operationId };
}