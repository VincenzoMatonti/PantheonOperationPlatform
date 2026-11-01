using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemManagement.Contracts;

public sealed class ExecutionAgentResponseException(string message)
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.InvalidResponse,
        "ExecutionAgent",
        message,
        ApplicationCoreErrorType.BadGateway);
