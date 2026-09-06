using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Entities;

public sealed class Operation
{
    private Operation()
    {
    }

    private Operation(
        Guid id,
        OperationType type,
        CorrelationId correlationId,
        ExternalOperationId externalId,
        Guid sourceEndpointId,
        Guid targetEndpointId)
    {
        Id = id;
        Type = type;
        CorrelationId = correlationId;
        ExternalId = externalId;
        SourceEndpointId = sourceEndpointId;
        TargetEndpointId = targetEndpointId;
        Status = OperationStatus.Received;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public OperationType Type { get; private set; } = null!;

    public CorrelationId CorrelationId { get; private set; } = null!;

    public ExternalOperationId ExternalId { get; private set; } = null!;

    public Guid SourceEndpointId { get; private set; }

    public Guid TargetEndpointId { get; private set; }

    public OperationStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Operation Create(
        OperationType type,
        CorrelationId correlationId,
        ExternalOperationId externalId,
        Guid sourceEndpointId,
        Guid targetEndpointId)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(correlationId);
        ArgumentNullException.ThrowIfNull(externalId);

        if (sourceEndpointId == Guid.Empty)
        {
            throw new ArgumentException("Source endpoint ID cannot be empty.", nameof(sourceEndpointId));
        }

        if (targetEndpointId == Guid.Empty)
        {
            throw new ArgumentException("Target endpoint ID cannot be empty.", nameof(targetEndpointId));
        }

        return new Operation(Guid.NewGuid(), type, correlationId, externalId, sourceEndpointId, targetEndpointId);
    }

    public void Validate()
    {
        if (Status != OperationStatus.Received)
        {
            throw new InvalidOperationException("Only received operations can be validated.");
        }

        Status = OperationStatus.Validated;
        UpdateTimestamp();
    }

    public void Accept()
    {
        if (Status != OperationStatus.Validated)
        {
            throw new InvalidOperationException("Only validated operations can be accepted.");
        }

        Status = OperationStatus.Accepted;
        UpdateTimestamp();
    }

    public void Reject()
    {
        if (Status != OperationStatus.Received && Status != OperationStatus.Validated)
        {
            throw new InvalidOperationException("Only received or validated operations can be rejected.");
        }

        Status = OperationStatus.Rejected;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}