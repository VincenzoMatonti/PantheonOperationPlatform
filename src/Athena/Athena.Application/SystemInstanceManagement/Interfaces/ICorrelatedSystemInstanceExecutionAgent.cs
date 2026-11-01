using Athena.Application.SystemInstanceManagement.Contracts;

namespace Athena.Application.SystemInstanceManagement.Interfaces;

public interface ICorrelatedSystemInstanceExecutionAgent
{
    Task<SystemInstanceExecutionResult> ExecuteAsync(SystemInstanceExecutionCommand command, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionResult> GetOperationAsync(Guid systemInstanceId, Guid operationId, CancellationToken cancellationToken = default);
}
