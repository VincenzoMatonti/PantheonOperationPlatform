using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemManagement.Contracts;

public sealed class ExecutionAgentRequestException(HttpRequestException innerException)
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.RequestFailed,
        "ExecutionAgent",
        $"The execution agent request failed: {innerException.Message}",
        ApplicationCoreErrorType.BadGateway)
{
    public HttpRequestException InnerRequestException { get; } = innerException;
}

public enum ExecutionAgentErrorCode
{
    NotConfigured = 1,
    InvalidResponse = 2,
    RequestFailed = 3,
    TimedOut = 4
}
