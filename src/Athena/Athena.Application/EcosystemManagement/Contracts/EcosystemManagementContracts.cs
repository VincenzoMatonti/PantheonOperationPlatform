namespace Athena.Application.EcosystemManagement.Contracts;

public sealed record EcosystemLifecycleDto(
    Guid EcosystemId,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);

public sealed record EcosystemOperationDto(
    Guid Id,
    Guid EcosystemId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    bool IsDeleted);

public interface IEcosystemManagement
{
    Task<EcosystemLifecycleDto> CreateLifecycleAsync(Guid ecosystemId, CancellationToken cancellationToken = default);
    Task<EcosystemLifecycleDto?> GetLifecycleAsync(Guid ecosystemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(CancellationToken cancellationToken = default);
    Task<EcosystemLifecycleDto> SetDesiredStateAsync(Guid ecosystemId, string desiredState, CancellationToken cancellationToken = default);
    Task<EcosystemOperationDto> ExecuteOperationAsync(Guid ecosystemId, string operationType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(Guid ecosystemId, CancellationToken cancellationToken = default);
}
