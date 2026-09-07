using Hermes.Domain.Executions.ValueObjects;

namespace Hermes.Domain.Executions.Entities;

public class ExecutionStep
{
    private ExecutionStep()
    {
    }

    private ExecutionStep(Guid id, Guid executionId, int sequence)
    {
        Id = id;
        ExecutionId = executionId;
        Sequence = sequence;
        Status = ExecutionStepStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ExecutionId { get; private set; }

    public int Sequence { get; private set; }

    public ExecutionStepStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? StartedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ExecutionStep Create(Guid executionId, int sequence)
    {
        if (executionId == Guid.Empty)
        {
            throw new ArgumentException("Execution ID cannot be empty.", nameof(executionId));
        }

        if (sequence < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "Sequence must be greater than zero.");
        }

        return new ExecutionStep(Guid.NewGuid(), executionId, sequence);
    }

    public void Start()
    {
        if (Status != ExecutionStepStatus.Pending)
        {
            throw new InvalidOperationException("Only pending execution steps can be started.");
        }

        Status = ExecutionStepStatus.Running;
        StartedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Complete()
    {
        if (Status != ExecutionStepStatus.Running)
        {
            throw new InvalidOperationException("Only running execution steps can be completed.");
        }

        Status = ExecutionStepStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Fail()
    {
        if (Status != ExecutionStepStatus.Running)
        {
            throw new InvalidOperationException("Only running execution steps can be failed.");
        }

        Status = ExecutionStepStatus.Failed;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Skip()
    {
        if (Status != ExecutionStepStatus.Pending)
        {
            throw new InvalidOperationException("Only pending execution steps can be skipped.");
        }

        Status = ExecutionStepStatus.Skipped;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Cancel()
    {
        if (Status != ExecutionStepStatus.Pending && Status != ExecutionStepStatus.Running)
        {
            throw new InvalidOperationException("Only pending or running execution steps can be cancelled.");
        }

        Status = ExecutionStepStatus.Cancelled;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}