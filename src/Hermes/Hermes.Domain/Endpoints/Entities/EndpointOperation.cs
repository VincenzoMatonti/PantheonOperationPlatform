using Hermes.Domain.Endpoints.Exceptions;

namespace Hermes.Domain.Endpoints.Entities;

public class EndpointOperation
{
    private EndpointOperation(Guid id, Guid endpointId, Guid operationTypeId)
    {
        Id = id;
        EndpointId = endpointId;
        OperationTypeId = operationTypeId;
        IsEnabled = true;
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid EndpointId { get; private set; }
    public Guid OperationTypeId { get; private set; }
    public bool IsEnabled { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static EndpointOperation Create(Guid endpointId, Guid operationTypeId)
    {
        if (endpointId == Guid.Empty) throw new EndpointOperationEndpointIdRequiredException();
        if (operationTypeId == Guid.Empty) throw new EndpointOperationOperationTypeIdRequiredException();
        return new EndpointOperation(
        Guid.NewGuid(), endpointId, operationTypeId);
    }

    public void Enable()
    {
        if (IsEnabled) throw new EndpointOperationAlreadyEnabledException(Id);
        IsEnabled = true;
        UpdateTimestamp();
    }

    public void Disable()
    {
        if (!IsEnabled) throw new EndpointOperationAlreadyDisabledException(Id);
        IsEnabled = false;
        UpdateTimestamp();
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted) throw new EndpointOperationAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdateTimestamp();
    }

    public void Restore()
    {
        if (!IsDeleted) throw new EndpointOperationNotDeletedException(Id);
        IsDeleted = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}