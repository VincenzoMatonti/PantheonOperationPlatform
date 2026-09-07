namespace Hermes.Domain.Endpoints.Entities;

public sealed class EndpointOperation
{
    private EndpointOperation()
    {
    }

    private EndpointOperation(Guid id, Guid endpointId, Guid operationTypeId)
    {
        Id = id;
        EndpointId = endpointId;
        OperationTypeId = operationTypeId;
        IsEnabled = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid EndpointId { get; private set; }

    public Guid OperationTypeId { get; private set; }

    public bool IsEnabled { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static EndpointOperation Create(Guid endpointId, Guid operationTypeId)
    {
        if (endpointId == Guid.Empty)
        {
            throw new ArgumentException("Endpoint ID cannot be empty.", nameof(endpointId));
        }

        if (operationTypeId == Guid.Empty)
        {
            throw new ArgumentException("OperationType ID cannot be empty.", nameof(operationTypeId));
        }

        return new EndpointOperation(
            Guid.NewGuid(), endpointId, operationTypeId);
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