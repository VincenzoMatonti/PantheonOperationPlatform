using Athena.Api.SystemManagement.Requests;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Contracts;
using Athena.Application.SystemManagement.Queries;
using Athena.Domain.System.Management.Enums;
using ConfigureRequest = Athena.Api.SystemManagement.Requests.ConfigureSystemInstanceRequest;
using ResolveRequest = Athena.Api.SystemManagement.Requests.ResolveSystemInstanceOperationRequest;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemInstanceProcessRequestMapper
{
    public static GetSystemInstancesQuery ToGetAllQuery() => new();
    public static GetSystemInstanceByIdQuery ToGetByIdQuery(Guid id) => new(id);
    public static CreateSystemInstanceCommand ToCreateCommand(CreateSystemInstanceRequest request) =>
        new(new SystemInstanceExecutionCommandRequest(request.SystemInstanceId, request.SystemId, request.WorkloadId, request.Name, request.Details, request.ConfigurationReference));
    public static RegisterSystemInstanceCommand ToRegisterCommand(RegisterSystemInstanceRequest request) =>
        new(new SystemInstanceExecutionCommandRequest(request.SystemInstanceId, Details: request.Details, ConfigurationReference: request.ConfigurationReference));
    public static EnqueueSystemInstanceOperationCommand ToOperationCommand(Guid id, SystemInstanceOperationType type) => new(id, type);
    public static ConfigureSystemInstanceCommand ToConfigureCommand(Guid id, ConfigureRequest request) =>
        new(id, request.ConfigurationReference);
    public static GetSystemInstanceStatusQuery ToStatusQuery(Guid id) => new(id);
    public static GetSystemInstanceHealthQuery ToHealthQuery(Guid id) => new(id);
    public static GetSystemInstanceLogsQuery ToLogsQuery(Guid id, GetSystemInstanceLogsRequest request) => new(id, request.Tail);
    public static GetSystemInstanceOperationsQuery ToOperationsQuery(Guid id) => new(id);
    public static GetSystemInstanceOperationQuery ToOperationQuery(Guid id, Guid operationId) => new(id, operationId);
    public static ResolveSystemInstanceOperationCommand ToResolveOutcomeUnknownCommand(
        Guid id,
        Guid operationId,
        ResolveRequest request) =>
        new(id, operationId, request.Reason);
}
