using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Clients.Entities;

public class ClientOperation
{
    private ClientOperation()
    {
    }

    private ClientOperation(Guid id, Guid clientId, OperationType operationType)
    {
        Id = id;
        ClientId = clientId;
        OperationType = operationType;
        IsEnabled = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ClientId { get; private set; }

    public OperationType OperationType { get; private set; } = null!;

    public bool IsEnabled { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ClientOperation Create(Guid clientId, OperationType operationType)
    {
        if (clientId == Guid.Empty)
        {
            throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
        }

        ArgumentNullException.ThrowIfNull(operationType);

        return new ClientOperation(Guid.NewGuid(), clientId, operationType);
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