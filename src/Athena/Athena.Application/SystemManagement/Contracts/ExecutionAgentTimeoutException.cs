using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemManagement.Contracts;

public sealed class ExecutionAgentTimeoutException(TaskCanceledException innerException)
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.TimedOut,
        "ExecutionAgent",
        "The execution agent request timed out.",
        ApplicationCoreErrorType.ServiceUnavailable)
{
    public TaskCanceledException InnerTimeoutException { get; } = innerException;
}
