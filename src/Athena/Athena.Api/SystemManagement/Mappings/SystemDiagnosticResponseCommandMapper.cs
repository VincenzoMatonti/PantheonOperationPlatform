using Athena.Api.SystemManagement.Responses;
using Athena.Application.Common.Dtos;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemDiagnosticResponseCommandMapper
{
    public static RuntimeResourceStatusResponse ToResponse(RuntimeResourceStatusDto dto)
    {
        return new RuntimeResourceStatusResponse
        {
            State = dto.State,
            IsRunning = dto.IsRunning,
            StartedAtUtc = dto.StartedAtUtc,
            ObservedAtUtc = dto.ObservedAtUtc,
            Details = dto.Details
        };
    }

    public static RuntimeHealthCheckResponse ToResponse(RuntimeHealthCheckDto dto)
    {
        return new RuntimeHealthCheckResponse
        {
            IsHealthy = dto.IsHealthy,
            Status = dto.Status,
            CheckedAtUtc = dto.CheckedAtUtc,
            Details = dto.Details
        };
    }

    public static IReadOnlyList<RuntimeLogEntryResponse> ToResponse(IReadOnlyList<RuntimeLogEntryDto> dtos)
    {
        return [.. dtos.Select(dto => new RuntimeLogEntryResponse
        {
            TimestampUtc = dto.TimestampUtc,
            Stream = dto.Stream,
            Message = dto.Message
        })];
    }
}