namespace Athena.Cli.Application.Contracts.Orchestrations;

public record CreateOrchestrationSystemRequest(Guid OrchestrationId, Guid SystemId);
public record GetOrchestrationSystemsRequest(bool IncludeDeleted = false);

