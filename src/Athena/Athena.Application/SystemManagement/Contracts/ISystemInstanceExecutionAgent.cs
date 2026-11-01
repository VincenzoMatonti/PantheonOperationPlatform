namespace Athena.Application.SystemManagement.Contracts;

public sealed record SystemInstanceExecutionCommandRequest(
    Guid SystemInstanceId,
    Guid? SystemId = null,
    Guid? WorkloadId = null,
    string? Name = null,
    string? Details = null);

public sealed record SystemInstanceProcessDto(
    Guid Id,
    Guid SystemId,
    Guid WorkloadId,
    string Name,
    string RegistrationStatus,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);

public sealed record SystemInstanceExecutionStatus(
    Guid SystemInstanceId,
    string State,
    bool IsRunning,
    bool IsHealthy,
    DateTime ObservedAtUtc,
    string? Details = null);

public sealed record SystemInstanceExecutionHealth(
    Guid SystemInstanceId,
    bool IsHealthy,
    string Status,
    DateTime CheckedAtUtc,
    string? Details = null);

public sealed record SystemInstanceExecutionLogEntry(
    DateTime TimestampUtc,
    string Stream,
    string Message);

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
