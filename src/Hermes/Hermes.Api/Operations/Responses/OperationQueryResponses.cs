using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Api.Operations.Responses;

public class OperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
}