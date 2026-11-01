using Athena.Application.SystemInstanceManagement.Contracts;

namespace Athena.Application.SystemInstanceManagement.Interfaces;

public interface ISystemInstanceProcessCoordinator
{
    Task<SystemInstanceProcessDto?> GetByIdAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task EnsureOperableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
}
