namespace Athena.Application.Common.Dtos;

public record RuntimeResourceReferenceDto(string Provider, string ResourceId, string Environment);

public record RuntimeResourceStatusDto(string State, bool IsRunning, DateTime? StartedAtUtc, DateTime ObservedAtUtc, string? Details = null);

public record RuntimeHealthCheckDto(bool IsHealthy, string Status, DateTime CheckedAtUtc, string? Details = null);

public record RuntimeLogEntryDto(DateTime TimestampUtc, string Stream, string Message);

public record RuntimeLogOptionsDto(int Tail = 100, DateTime? SinceUtc = null, DateTime? UntilUtc = null, bool Follow = false);

public record RuntimeOperationResultDto(bool Succeeded, string? Details = null);