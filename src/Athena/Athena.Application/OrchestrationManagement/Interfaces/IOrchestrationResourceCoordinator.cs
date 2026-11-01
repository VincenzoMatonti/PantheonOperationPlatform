using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.Interfaces;

public sealed record OrchestrationResourceLifecycleSummary(
    ResourceAggregateStatus AggregateStatus,
    string DesiredState,
    string OperationalStatus,
    DateTime? LatestObservedAtUtc);

public interface IOrchestrationResourceCoordinator
{
    Task<IReadOnlyList<Guid>> ExecuteAsync(
        Guid orchestrationId,
        IReadOnlyCollection<Guid> systemInstanceIds,
        string operationType,
        CancellationToken cancellationToken = default);

    Task<OrchestrationResourceLifecycleSummary> GetLifecycleSummaryAsync(
        IEnumerable<Guid> systemInstanceIds,
        CancellationToken cancellationToken = default);
}
