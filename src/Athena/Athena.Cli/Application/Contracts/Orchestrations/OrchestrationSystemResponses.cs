namespace Athena.Cli.Application.Contracts.Orchestrations;

public class OrchestrationSystemResponse
{
    public Guid Id { get; init; }

    public Guid OrchestrationId { get; init; }

    public Guid SystemId { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public bool IsDeleted { get; init; }
}
