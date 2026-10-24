namespace Athena.Cli.Application.Contracts.Systems;

public record ReadRuntimeLogsRequest(
    int Tail = 100,
    DateTime? SinceUtc = null,
    DateTime? UntilUtc = null);
