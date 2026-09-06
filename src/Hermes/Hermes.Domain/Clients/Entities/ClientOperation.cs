using Hermes.Domain.Operations.Entities;

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
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ClientId { get; private set; }

    public Guid OperationTypeId { get; private set; }

    public bool IsEnabled { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ClientOperation Create(Guid clientId, Guid operationTypeId)
    {
        if (clientId == Guid.Empty)
        {
            throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
        }

        if (operationTypeId == Guid.Empty)
        {
            throw new ArgumentException("Operation type ID cannot be empty.", nameof(operationTypeId));
        }

        return new ClientOperation(Guid.NewGuid(), clientId, operationTypeId);
    }

    public void Enable()
    {
        if (IsEnabled)
        {
            return;
        }

        IsEnabled = true;
        UpdateTimestamp();
    }

    public void Disable()
    {
        if (!IsEnabled)
        {
            return;
        }

        IsEnabled = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}