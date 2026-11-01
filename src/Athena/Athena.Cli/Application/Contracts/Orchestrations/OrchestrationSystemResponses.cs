namespace Athena.Cli.Application.Contracts.Orchestrations;

public class OrchestrationSystemInstanceResponse
{
    public Guid Id { get; init; }

    public Guid OrchestrationId { get; init; }

    public Guid SystemInstanceId { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public bool IsDeleted { get; init; }
}
