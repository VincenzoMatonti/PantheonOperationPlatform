namespace Athena.Application.SystemManagement.Commands;

public record GetRuntimeStatusCommand(Guid RuntimeInstanceId);
public record CheckRuntimeHealthCommand(Guid RuntimeInstanceId);
public record ReadRuntimeLogsCommand(Guid RuntimeInstanceId, int Tail = 100, DateTime? SinceUtc = null, DateTime? UntilUtc = null);