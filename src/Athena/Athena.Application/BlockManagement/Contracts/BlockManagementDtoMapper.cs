using Athena.Domain.Block.Management.Entities;

namespace Athena.Application.BlockManagement.Contracts;

public static class BlockManagementDtoMapper
{
    public static BlockLifecycleDto ToDto(BlockLifecycle lifecycle) =>
        new(lifecycle.Id.Value, lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(),
            lifecycle.CreatedAtUtc, lifecycle.UpdatedAtUtc, lifecycle.IsDeleted);

    public static BlockOperationDto ToDto(BlockOperation operation) =>
        new(operation.Id, operation.BlockId.Value, operation.Type.ToString(), operation.Status.ToString(),
            operation.CreatedAtUtc, operation.StartedAtUtc, operation.CompletedAtUtc,
            operation.ErrorMessage, operation.IsDeleted);
}
