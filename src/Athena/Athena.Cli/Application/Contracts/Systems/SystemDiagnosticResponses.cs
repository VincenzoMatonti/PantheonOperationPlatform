namespace Athena.Cli.Application.Contracts.Systems;

public class RuntimeResourceStatusResponse
{
    public string State { get; init; } = string.Empty;
    public bool IsRunning { get; init; }
    public DateTime? StartedAtUtc { get; init; }
    public DateTime ObservedAtUtc { get; init; }
    public string? Details { get; init; }
}

public class RuntimeHealthCheckResponse
{
    public bool IsHealthy { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CheckedAtUtc { get; init; }
    public string? Details { get; init; }
}

public class RuntimeLogEntryResponse
{
    public DateTime TimestampUtc { get; init; }
    public string Stream { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
