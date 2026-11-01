namespace Athena.Cli.Application.Contracts.Orchestrations;

public record CreateOrchestrationSystemInstanceRequest(Guid OrchestrationId, Guid SystemInstanceId);
public record GetOrchestrationSystemInstancesRequest(bool IncludeDeleted = false);

