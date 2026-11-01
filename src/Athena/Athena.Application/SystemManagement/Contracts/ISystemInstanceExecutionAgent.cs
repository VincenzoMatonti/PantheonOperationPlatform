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
    DateTime? UpdatedAtUtc,
    DateTime? ObservedAtUtc = null);

public sealed record SystemInstanceExecutionStatus(
    Guid SystemInstanceId,
    string State,
    bool IsRunning,
    bool IsHealthy,
    DateTime ObservedAtUtc,
    string? Details = null);

public sealed record SystemInstanceExecutionCommand(
    Guid OperationId,
    Guid SystemInstanceId,
    string Command);

public enum AgentOperationStatus
{
    Accepted = 0,
    Running = 1,
    Completed = 2,
    Failed = 3,
    NotFound = 4
}

public sealed record SystemInstanceExecutionResult(
    Guid OperationId,
    Guid SystemInstanceId,
    AgentOperationStatus Status,
    DateTime UpdatedAtUtc,
    DateTime? AcceptedAtUtc = null,
    DateTime? CompletedAtUtc = null,
    string? ErrorMessage = null,
    string? ObservedState = null,
    DateTime? ObservedAtUtc = null);

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

public interface ICorrelatedSystemInstanceExecutionAgent
{
    Task<SystemInstanceExecutionResult> ExecuteAsync(SystemInstanceExecutionCommand command, CancellationToken cancellationToken = default);
    Task<SystemInstanceExecutionResult> GetOperationAsync(Guid systemInstanceId, Guid operationId, CancellationToken cancellationToken = default);
}
