namespace Athena.Application.BlockManagement.Contracts;

public sealed record BlockLifecycleDto(
    Guid BlockId,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);

public sealed record BlockOperationDto(
    Guid Id,
    Guid BlockId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    bool IsDeleted);

public interface IBlockManagement
{
    Task<BlockLifecycleDto> CreateLifecycleAsync(Guid blockId, CancellationToken cancellationToken = default);
    Task<BlockLifecycleDto?> GetLifecycleAsync(Guid blockId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockLifecycleDto>> GetLifecyclesAsync(CancellationToken cancellationToken = default);
    Task<BlockLifecycleDto> SetDesiredStateAsync(Guid blockId, string desiredState, CancellationToken cancellationToken = default);
    // Completion confirms that intent was persisted; it does not operate the Block.
    Task<BlockOperationDto> ExecuteOperationAsync(Guid blockId, string operationType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockOperationDto>> GetOperationsAsync(Guid blockId, CancellationToken cancellationToken = default);
}
