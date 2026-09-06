using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Entities;

public sealed class Operation
{
    private Operation()
    {
    }

    private Operation(
        Guid id,
        Guid operationTypeId,
        CorrelationId correlationId,
        ExternalOperationId externalId,
        Guid clientId,
        Guid endpointId)
    {
        Id = id;
        OperationTypeId = operationTypeId;
        CorrelationId = correlationId;
        ExternalId = externalId;
        ClientId = clientId;
        EndpointId = endpointId;
        Status = OperationStatus.Received;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid OperationTypeId { get; private set; } 

    public CorrelationId CorrelationId { get; private set; } = null!;

    public ExternalOperationId ExternalId { get; private set; } = null!;

    public Guid ClientId { get; private set; }

    public Guid EndpointId { get; private set; }

    public OperationStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Operation Create(
        Guid operationTypeId,
        CorrelationId correlationId,
        ExternalOperationId externalId,
        Guid clientId,
        Guid endpointId)
    {

        ArgumentNullException.ThrowIfNull(correlationId);
        ArgumentNullException.ThrowIfNull(externalId);
        
        if (operationTypeId == Guid.Empty)
        {
            throw new ArgumentException("OperationTypeId ID cannot be empty.", nameof(operationTypeId));
        }

  
        if (clientId == Guid.Empty)
        {
            throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
        }

        if (endpointId == Guid.Empty)
        {
            throw new ArgumentException("Endpoint ID cannot be empty.", nameof(endpointId));
        }

        return new Operation(Guid.NewGuid(), operationTypeId, correlationId, externalId, clientId, endpointId);
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