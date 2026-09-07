using Hermes.Domain.Executions.ValueObjects;

namespace Hermes.Domain.Executions.Entities;

public class Execution
{
    private Execution()
    {
    }

    private Execution(Guid id)
    {
        Id = id;
        Status = ExecutionStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public ExecutionStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? StartedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Execution Create()
    {
        return new Execution(Guid.NewGuid());
    }

    public void Start()
    {
        if (Status != ExecutionStatus.Pending)
        {
            throw new InvalidOperationException("Only pending executions can be started.");
        }

        Status = ExecutionStatus.Running;
        StartedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Complete()
    {
        if (Status != ExecutionStatus.Running)
        {
            throw new InvalidOperationException("Only running executions can be completed.");
        }

        Status = ExecutionStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Fail()
    {
        if (Status != ExecutionStatus.Running)
        {
            throw new InvalidOperationException("Only running executions can be failed.");
        }

        Status = ExecutionStatus.Failed;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    public void Cancel()
    {
        if (Status != ExecutionStatus.Pending && Status != ExecutionStatus.Running)
        {
            throw new InvalidOperationException("Only pending or running executions can be cancelled.");
        }

        Status = ExecutionStatus.Cancelled;
        CompletedAt = DateTimeOffset.UtcNow;

        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}