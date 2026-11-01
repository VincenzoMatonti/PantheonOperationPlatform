namespace Athena.Application.OrchestrationManagement.Enums;

public enum ResourceAggregateStatus
{
    Unknown = 0,
    Converged = 1,
    PartiallyConverged = 2,
    Transitioning = 3,
    Diverged = 4,
    Unreachable = 5,
    Failed = 6,
    Stale = 7,
    OutcomeUnknown = 8
}
