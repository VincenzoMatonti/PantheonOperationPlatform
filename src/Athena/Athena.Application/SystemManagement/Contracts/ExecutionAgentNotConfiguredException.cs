using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemManagement.Contracts;

public sealed class ExecutionAgentNotConfiguredException()
    : ApplicationException<ExecutionAgentErrorCode>(
        ExecutionAgentErrorCode.NotConfigured,
        "ExecutionAgent",
        "No execution agent endpoint is configured. Set Athena:ExecutionAgent:BaseUrl before issuing SystemInstance process commands.",
        ApplicationCoreErrorType.ServiceUnavailable);
