using Athena.Application.SystemInstanceManagement.Contracts;

namespace Athena.Application.SystemInstanceManagement.Interfaces;

public interface ISystemInstanceExecutionAgent
{
    Task<SystemInstanceExecutionStatus> CreateAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> RegisterAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> RemoveAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(Guid systemInstanceId, int tail = 100, CancellationToken cancellationToken = default);
}

