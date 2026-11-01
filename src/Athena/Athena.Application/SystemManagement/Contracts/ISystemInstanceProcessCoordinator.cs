namespace Athena.Application.SystemManagement.Contracts;

public interface ISystemInstanceProcessCoordinator
{
    Task EnsureOperableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default);
}
