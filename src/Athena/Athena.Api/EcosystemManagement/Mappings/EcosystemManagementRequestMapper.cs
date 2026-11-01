using Athena.Api.EcosystemManagement.Requests;
using Athena.Application.EcosystemManagement.Commands;
using Athena.Application.EcosystemManagement.Queries;
using Athena.Domain.Ecosystem.Management.Enums;

namespace Athena.Api.EcosystemManagement.Mappings;

public static class EcosystemManagementRequestMapper
{
    public static CreateEcosystemLifecycleCommand ToCreateLifecycleCommand(Guid ecosystemId) => new(ecosystemId);
    public static GetEcosystemLifecycleQuery ToGetLifecycleQuery(Guid ecosystemId) => new(ecosystemId);
    public static GetEcosystemLifecyclesQuery ToGetLifecyclesQuery() => new();
    public static SetEcosystemDesiredStateCommand ToSetDesiredStateCommand(Guid ecosystemId, SetEcosystemDesiredStateRequest request) =>
        new(ecosystemId, request.DesiredState);
    public static ExecuteEcosystemOperationCommand ToExecuteOperationCommand(Guid ecosystemId, EcosystemOperationType operationType) =>
        new(ecosystemId, operationType.ToString());
    public static GetEcosystemOperationsQuery ToGetOperationsQuery(Guid ecosystemId) => new(ecosystemId);
}
