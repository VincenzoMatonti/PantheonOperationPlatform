namespace Hermes.Domain.Executions.Entities;

public class ExecutionStepRoute
{
    private ExecutionStepRoute()
    {
    }

    private ExecutionStepRoute(Guid id, Guid executionStepId, Guid routeId)
    {
        Id = id;
        ExecutionStepId = executionStepId;
        RouteId = routeId;
        IsEnabled = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ExecutionStepId { get; private set; }

    public Guid RouteId { get; private set; }

    public bool IsEnabled { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ExecutionStepRoute Create(Guid executionStepId, Guid routeId)
    {
        if (executionStepId == Guid.Empty)
        {
            throw new ArgumentException("Execution step ID cannot be empty.", nameof(executionStepId));
        }

        if (routeId == Guid.Empty)
        {
            throw new ArgumentException("Route ID cannot be empty.", nameof(routeId));
        }

        return new ExecutionStepRoute(Guid.NewGuid(), executionStepId, routeId);
    }

    public void Enable()
    {
        if (IsEnabled)
        {
            return;
        }

        IsEnabled = true;

        UpdateTimestamp();
    }

    public void Disable()
    {
        if (!IsEnabled)
        {
            return;
        }

        IsEnabled = false;

        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}