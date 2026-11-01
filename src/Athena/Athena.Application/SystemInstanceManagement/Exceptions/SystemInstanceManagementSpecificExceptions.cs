using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemInstanceManagement.Exceptions;

public enum ExecutionAgentErrorCode
{
    NotConfigured = 1,
    InvalidResponse = 2,
    RequestFailed = 3,
    TimedOut = 4
}

public sealed class ExecutionAgentNotConfiguredException()
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.NotConfigured,
        "ExecutionAgent",
        "No execution agent endpoint is configured. Set Athena:ExecutionAgent:BaseUrl before issuing SystemInstance process commands.",
        ApplicationCoreErrorType.ServiceUnavailable);

public sealed class ExecutionAgentRequestException(HttpRequestException innerException)
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.RequestFailed,
        "ExecutionAgent",
        $"The execution agent request failed: {innerException.Message}",
        ApplicationCoreErrorType.BadGateway)
{
    public HttpRequestException InnerRequestException { get; } = innerException;
}

public sealed class ExecutionAgentResponseException(string message)
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.InvalidResponse,
        "ExecutionAgent",
        message,
        ApplicationCoreErrorType.BadGateway);

public sealed class ExecutionAgentTimeoutException(TaskCanceledException innerException)
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.TimedOut,
        "ExecutionAgent",
        "The execution agent request timed out.",
        ApplicationCoreErrorType.ServiceUnavailable)
{
    public TaskCanceledException InnerTimeoutException { get; } = innerException;
}