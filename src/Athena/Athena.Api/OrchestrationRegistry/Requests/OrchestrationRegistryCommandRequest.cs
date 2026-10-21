namespace Athena.Api.OrchestrationRegistry.Requests;

public class CreateOrchestrationRequest
{
    public Guid OrchestrationId { get; init; }

    public string Name { get; init; } = string.Empty;
}

public class RenameOrchestrationRequest
{
    public string Name { get; init; } = string.Empty;
}