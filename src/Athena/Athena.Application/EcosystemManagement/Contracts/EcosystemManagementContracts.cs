namespace Athena.Application.EcosystemManagement.Contracts;

using Athena.Application.EcosystemManagement.Commands;
using Athena.Application.EcosystemManagement.Queries;

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
    Task<EcosystemLifecycleDto> CreateLifecycleAsync(CreateEcosystemLifecycleCommand command, CancellationToken cancellationToken = default);
    Task<EcosystemLifecycleDto?> GetLifecycleAsync(GetEcosystemLifecycleQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(GetEcosystemLifecyclesQuery query, CancellationToken cancellationToken = default);
    Task<EcosystemLifecycleDto> SetDesiredStateAsync(SetEcosystemDesiredStateCommand command, CancellationToken cancellationToken = default);
    Task<EcosystemOperationDto> ExecuteOperationAsync(ExecuteEcosystemOperationCommand command, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(GetEcosystemOperationsQuery query, CancellationToken cancellationToken = default);

    Task<EcosystemLifecycleDto> CreateLifecycleAsync(Guid ecosystemId, CancellationToken cancellationToken = default);
    Task<EcosystemLifecycleDto?> GetLifecycleAsync(Guid ecosystemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(CancellationToken cancellationToken = default);
    Task<EcosystemLifecycleDto> SetDesiredStateAsync(Guid ecosystemId, string desiredState, CancellationToken cancellationToken = default);
    // Completion confirms that intent was persisted; it does not operate the Ecosystem.
    Task<EcosystemOperationDto> ExecuteOperationAsync(Guid ecosystemId, string operationType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(Guid ecosystemId, CancellationToken cancellationToken = default);
}
