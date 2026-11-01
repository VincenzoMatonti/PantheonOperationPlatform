namespace Athena.Application.OrchestrationManagement.Interfaces;

public sealed record OrchestrationResourceLifecycleSummary(string DesiredState, string OperationalStatus);

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
