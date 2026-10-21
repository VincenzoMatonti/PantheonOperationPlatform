namespace Athena.Cli.Application.Contracts.Orchestrations;

public record CreateOrchestrationRequest(Guid Id, string Name);

public record RenameOrchestrationRequest(string Name);

public record GetOrchestrationByNameRequest(string Name);
