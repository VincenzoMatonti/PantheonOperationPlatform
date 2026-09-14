using Hermes.Domain.Operations.Exceptions;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Entities;

public class Operation
{
    private Operation(Guid id, Guid executionId, CorrelationId correlationId, ExternalOperationId externalId)
    {
        Id = id;
        ExecutionId = executionId;
        CorrelationId = correlationId;
        ExternalId = externalId;
        Status = OperationStatus.Initialized;
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ExecutionId { get; private set; }

    public CorrelationId CorrelationId { get; private set; } = null!;

    public ExternalOperationId ExternalId { get; private set; } = null!;

    public OperationStatus Status { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Operation Create(Guid executionId, CorrelationId correlationId, ExternalOperationId externalId)
    {
        if (executionId == Guid.Empty) throw new OperationExecutionIdRequiredException();
        if (correlationId is null) throw new OperationCorrelationIdRequiredException();
        if (externalId is null) throw new OperationExternalIdRequiredException();
        return new Operation(Guid.NewGuid(), executionId, correlationId, externalId);
    }

    public void Send()
    {
        if (Status != OperationStatus.Initialized) throw new OperationInvalidSendStateException(Id);
        Status = OperationStatus.Sent;
        UpdateTimestamp();
    }

    public void Validate()
    {
        if (Status != OperationStatus.Sent) throw new OperationInvalidValidationStateException(Id);
        Status = OperationStatus.Validated;
        UpdateTimestamp();
    }

    public void Accept()
    {
        if (Status != OperationStatus.Validated) throw new OperationInvalidAcceptanceStateException(Id);
        Status = OperationStatus.Accepted;
        UpdateTimestamp();
    }

    public void Reject()
    {
        if (Status != OperationStatus.Sent && Status != OperationStatus.Validated)
            throw new OperationInvalidRejectionStateException(Id);
        Status = OperationStatus.Rejected;
        UpdateTimestamp();
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted) throw new OperationAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdateTimestamp();
    }

    public void Restore()
    {
        if (!IsDeleted) throw new OperationNotDeletedException(Id);
        IsDeleted = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}