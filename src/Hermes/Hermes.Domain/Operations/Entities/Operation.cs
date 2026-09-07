using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Entities;

public class Operation
{
    private Operation()
    {
    }

    private Operation(Guid id, Guid executionId, CorrelationId correlationId, ExternalOperationId externalId)
    {
        Id = id;
        ExecutionId = executionId;
        CorrelationId = correlationId;
        ExternalId = externalId;
        Status = OperationStatus.Received;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ExecutionId { get; private set; }

    public CorrelationId CorrelationId { get; private set; } = null!;

    public ExternalOperationId ExternalId { get; private set; } = null!;

    public OperationStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Operation Create(Guid executionId, CorrelationId correlationId, ExternalOperationId externalId)
    {
        ArgumentNullException.ThrowIfNull(correlationId);
        ArgumentNullException.ThrowIfNull(externalId);

        if (executionId == Guid.Empty)
        {
            throw new ArgumentException("Execution ID cannot be empty.", nameof(executionId));
        }

        return new Operation(Guid.NewGuid(), executionId, correlationId, externalId);
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