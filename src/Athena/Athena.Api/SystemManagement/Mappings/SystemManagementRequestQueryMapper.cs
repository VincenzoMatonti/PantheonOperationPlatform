using Athena.Api.SystemManagement.Requests;
using Athena.Application.SystemManagement.Queries;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemManagementRequestQueryMapper
{
    public static GetSystemLifecycleBySystemIdQuery ToLifecycleBySystemIdQuery(Guid systemId) => new()
    {
        SystemId = systemId
    };

    public static GetSystemLifecyclesByDesiredStateQuery ToLifecyclesByDesiredStateQuery(GetSystemLifecyclesByDesiredStateRequest request) => new()
    {
        DesiredState = request.DesiredState
    };

    public static GetSystemLifecyclesByOperationalStatusQuery ToLifecyclesByOperationalStatusQuery(GetSystemLifecyclesByOperationalStatusRequest request) => new()
    {
        OperationalStatus = request.OperationalStatus
    };

    public static GetSystemLifecyclesByDeletionStatusQuery ToLifecyclesByDeletionStatusQuery(GetSystemLifecyclesByDeletionStatusRequest request) => new()
    {
        IsDeleted = request.IsDeleted
    };

    public static GetSystemLifecycleOperationByIdQuery ToOperationByIdQuery(Guid operationId) => new()
    {
        OperationId = operationId
    };

    public static GetSystemLifecycleOperationsBySystemIdQuery ToOperationsBySystemIdQuery(Guid systemId) => new()
    {
        SystemId = systemId
    };

    public static GetSystemLifecycleOperationsByStatusQuery ToOperationsByStatusQuery(GetSystemLifecycleOperationsByStatusRequest request) => new()
    {
        Status = request.Status
    };

    public static GetSystemLifecycleOperationsBySystemIdAndStatusQuery ToOperationsBySystemIdAndStatusQuery(Guid systemId, GetSystemLifecycleOperationsBySystemIdAndStatusRequest request) => new()
    {
        SystemId = systemId,
        Status = request.Status
    };

    public static GetSystemLifecycleOperationsByDeletionStatusQuery ToOperationsByDeletionStatusQuery(GetSystemLifecycleOperationsByDeletionStatusRequest request) => new()
    {
        IsDeleted = request.IsDeleted
    };
}