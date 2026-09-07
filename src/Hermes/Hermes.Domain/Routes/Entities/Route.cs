namespace Hermes.Domain.Routes.Entities;

public class Route
{
    private Route()
    {
    }

    private Route(Guid id, Guid clientId, Guid operationTypeId, Guid endpointId)
    {
        Id = id;
        ClientId = clientId;
        OperationTypeId = operationTypeId;
        EndpointId = endpointId;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ClientId { get; private set; }

    public Guid OperationTypeId { get; private set; }

    public Guid EndpointId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Route Create(Guid clientId, Guid operationTypeId, Guid endpointId)
    {
        if (clientId == Guid.Empty)
        {
            throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
        }

        if (operationTypeId == Guid.Empty)
        {
            throw new ArgumentException("OperationType ID cannot be empty.", nameof(operationTypeId));
        }

        if (endpointId == Guid.Empty)
        {
            throw new ArgumentException("Endpoint ID cannot be empty.", nameof(endpointId));
        }

        return new Route(Guid.NewGuid(), clientId, operationTypeId, endpointId);
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}