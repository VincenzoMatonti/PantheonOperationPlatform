using Hermes.Domain.Clients.Exceptions;

namespace Hermes.Domain.Clients.Entities;

public class ClientOperation
{
    private ClientOperation()
    {
    }

    private ClientOperation(Guid id, Guid clientId, Guid operationTypeId)
    {
        Id = id;
        ClientId = clientId;
        OperationTypeId = operationTypeId;
        IsEnabled = true;
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ClientId { get; private set; }

    public Guid OperationTypeId { get; private set; }

    public bool IsEnabled { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ClientOperation Create(Guid clientId, Guid operationTypeId)
    {
        if (clientId == Guid.Empty) throw new ClientOperationClientIdRequiredException();
        if (operationTypeId == Guid.Empty) throw new ClientOperationTypeIdRequiredException();
        return new ClientOperation(Guid.NewGuid(), clientId, operationTypeId);
    }

    public void Enable()
    {
        if (IsEnabled) throw new ClientOperationAlreadyEnabledException(Id);
        IsEnabled = true;
        UpdateTimestamp();
    }

    public void Disable()
    {
        if (!IsEnabled) throw new ClientOperationAlreadyDisabledException(Id);
        IsEnabled = false;
        UpdateTimestamp();
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted) throw new ClientOperationAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdateTimestamp();
    }

    public void Restore()
    {
        if (!IsDeleted) throw new ClientOperationNotDeletedException(Id);
        IsDeleted = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}