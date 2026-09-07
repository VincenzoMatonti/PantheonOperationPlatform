namespace Hermes.Domain.Executions.Entities;

public class ExecutionStepType
{
    private ExecutionStepType()
    {
    }

    private ExecutionStepType(Guid id, Guid executionStepId, Guid executionTypeId)
    {
        Id = id;
        ExecutionStepId = executionStepId;
        ExecutionTypeId = executionTypeId;
        IsEnabled = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ExecutionStepId { get; private set; }

    public Guid ExecutionTypeId { get; private set; }

    public bool IsEnabled { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ExecutionStepType Create(Guid executionStepId, Guid executionTypeId)
    {
        if (executionStepId == Guid.Empty)
        {
            throw new ArgumentException("Execution step ID cannot be empty.", nameof(executionStepId));
        }

        if (executionTypeId == Guid.Empty)
        {
            throw new ArgumentException("Execution type ID cannot be empty.", nameof(executionTypeId));
        }

        return new ExecutionStepType(Guid.NewGuid(), executionStepId, executionTypeId);
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