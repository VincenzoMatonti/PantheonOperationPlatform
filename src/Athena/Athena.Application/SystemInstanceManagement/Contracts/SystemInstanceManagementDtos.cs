using Athena.Application.SystemInstanceManagement.Enums;

namespace Athena.Application.SystemInstanceManagement.Contracts;

public sealed record SystemInstanceExecutionResult(
    Guid OperationId,
    Guid SystemInstanceId,
    AgentOperationStatus Status,
    DateTime UpdatedAtUtc,
    DateTime? AcceptedAtUtc = null,
    DateTime? CompletedAtUtc = null,
    string? ErrorMessage = null,
    string? ObservedState = null,
    DateTime? ObservedAtUtc = null,
    Guid? CorrelationId = null);

public sealed record SystemInstanceExecutionHealth(Guid SystemInstanceId, bool IsHealthy, string Status, DateTime CheckedAtUtc, string? Details = null);

public sealed record SystemInstanceExecutionLogEntry(DateTime TimestampUtc, string Stream, string Message);

public sealed record SystemInstanceExecutionCommandRequest(
Guid SystemInstanceId,
Guid? SystemId = null,
Guid? WorkloadId = null,
string? Name = null,
string? Details = null,
string? ConfigurationReference = null);

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
    string Command,
    string? PayloadJson = null,
    Guid? SystemId = null,
    Guid? WorkloadId = null,
    string? Name = null,
    Guid? CorrelationId = null,
    string ContractVersion = "1.0");