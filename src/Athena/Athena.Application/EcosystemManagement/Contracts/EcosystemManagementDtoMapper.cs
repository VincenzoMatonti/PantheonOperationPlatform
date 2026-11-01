using Athena.Domain.Ecosystem.Management.Entities;

namespace Athena.Application.EcosystemManagement.Contracts;

public static class EcosystemManagementDtoMapper
{
    public static EcosystemLifecycleDto ToDto(EcosystemLifecycle lifecycle) =>
        new(lifecycle.Id.Value, lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(),
            lifecycle.CreatedAtUtc, lifecycle.UpdatedAtUtc, lifecycle.IsDeleted);

    public static EcosystemOperationDto ToDto(EcosystemOperation operation) =>
        new(operation.Id, operation.EcosystemId.Value, operation.Type.ToString(), operation.Status.ToString(),
            operation.CreatedAtUtc, operation.StartedAtUtc, operation.CompletedAtUtc,
            operation.ErrorMessage, operation.IsDeleted);
}
