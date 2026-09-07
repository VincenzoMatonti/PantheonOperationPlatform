namespace Hermes.Domain.Executions.ValueObjects;

public enum ExecutionStepStatus
{
    Pending = 1,
    Running = 2,
    Completed = 3,
    Failed = 4,
    Skipped = 5,
    Cancelled = 6
}