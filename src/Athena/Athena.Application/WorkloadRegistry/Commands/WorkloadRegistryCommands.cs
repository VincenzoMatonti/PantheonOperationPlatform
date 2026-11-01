namespace Athena.Application.WorkloadRegistry.Commands;

public sealed record CreateWorkloadCommand(Guid BlockId, string Name);

public sealed record RegisterWorkloadCommand(Guid Id);
public sealed record UnregisterWorkloadCommand(Guid Id);
public sealed record RenameWorkloadCommand(Guid Id, string Name);
public sealed record EnableWorkloadCommand(Guid Id);
public sealed record DisableWorkloadCommand(Guid Id);
public sealed record DeleteWorkloadCommand(Guid Id);

public sealed record MarkWorkloadAvailableCommand(Guid Id);
public sealed record MarkWorkloadUnavailableCommand(Guid Id);
public sealed record MarkWorkloadAvailabilityUnknownCommand(Guid Id);
public sealed record MarkWorkloadErrorCommand(Guid Id);