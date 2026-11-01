namespace Athena.Application.WorkloadManagement.Contracts;

public sealed record WorkloadLifecycleDto(
    Guid WorkloadId,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);

public sealed record WorkloadOperationDto(
    Guid Id,
    Guid WorkloadId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    bool IsDeleted);

public interface IWorkloadManagement
{
    Task<WorkloadLifecycleDto> CreateLifecycleAsync(Guid workloadId, CancellationToken cancellationToken = default);
    Task<WorkloadLifecycleDto?> GetLifecycleAsync(Guid workloadId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadLifecycleDto>> GetLifecyclesAsync(CancellationToken cancellationToken = default);
    Task<WorkloadLifecycleDto> SetDesiredStateAsync(Guid workloadId, string desiredState, CancellationToken cancellationToken = default);
    Task<WorkloadOperationDto> ExecuteOperationAsync(Guid workloadId, string operationType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadOperationDto>> GetOperationsAsync(Guid workloadId, CancellationToken cancellationToken = default);
}
