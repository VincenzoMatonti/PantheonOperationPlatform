using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.Contracts;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemInstanceProcessResponseMapper
{
    public static SystemInstanceResponse ToResponse(SystemInstanceProcessDto dto) =>
        new(dto.Id, dto.SystemId, dto.WorkloadId, dto.Name, dto.RegistrationStatus, dto.DesiredState,
            dto.OperationalStatus, dto.CreatedAtUtc, dto.UpdatedAtUtc, dto.ObservedAtUtc);

    public static IReadOnlyList<SystemInstanceResponse> ToResponses(IEnumerable<SystemInstanceProcessDto> dtos) =>
        dtos.Select(ToResponse).ToArray();

    public static SystemInstanceOperationResponse ToResponse(SystemInstanceOperationDto dto) =>
        new(dto.Id, dto.SystemInstanceId, dto.Type, dto.Status, dto.CreatedAtUtc, dto.StartedAtUtc,
            dto.CompletedAtUtc, dto.ErrorMessage, dto.AgentAcceptedAtUtc, dto.AgentCompletedAtUtc,
            dto.LeaseExpiresAtUtc, dto.AttemptCount, dto.ConvergenceStatus, dto.MaxAttempts);

    public static IReadOnlyList<SystemInstanceOperationResponse> ToOperationResponses(IEnumerable<SystemInstanceOperationDto> dtos) =>
        dtos.Select(ToResponse).ToArray();

    public static SystemInstanceExecutionStatusResponse ToResponse(SystemInstanceExecutionStatus dto) =>
        new(dto.SystemInstanceId, dto.State, dto.IsRunning, dto.IsHealthy, dto.ObservedAtUtc, dto.Details);

    public static SystemInstanceExecutionHealthResponse ToResponse(SystemInstanceExecutionHealth dto) =>
        new(dto.SystemInstanceId, dto.IsHealthy, dto.Status, dto.CheckedAtUtc, dto.Details);

    public static IReadOnlyList<SystemInstanceExecutionLogResponse> ToLogResponses(IEnumerable<SystemInstanceExecutionLogEntry> dtos) =>
        dtos.Select(dto => new SystemInstanceExecutionLogResponse(dto.TimestampUtc, dto.Stream, dto.Message)).ToArray();
}
