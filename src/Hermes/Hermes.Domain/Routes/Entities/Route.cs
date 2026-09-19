using Hermes.Domain.Routes.Exceptions;

namespace Hermes.Domain.Routes.Entities;

public class Route
{
    private Route(Guid id, Guid clientId, Guid operationTypeId, Guid endpointId)
    {
        Id = id;
        ClientId = clientId;
        OperationTypeId = operationTypeId;
        EndpointId = endpointId;
        IsActive = true;
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid ClientId { get; private set; }

    public Guid OperationTypeId { get; private set; }

    public Guid EndpointId { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Route Create(Guid clientId, Guid operationTypeId, Guid endpointId)
    {
        if (clientId == Guid.Empty) throw new RouteClientIdRequiredException();
        if (operationTypeId == Guid.Empty) throw new RouteOperationTypeIdRequiredException();
        if (endpointId == Guid.Empty) throw new RouteEndpointIdRequiredException();
        return new Route(Guid.NewGuid(), clientId, operationTypeId, endpointId);
    }

    public void Activate()
    {
        if (IsActive) throw new RouteAlreadyActiveException();
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        if (!IsActive) throw new RouteAlreadyInactiveException();
        IsActive = false;
        UpdateTimestamp();
    }

    public void Delete()
    {
        if (IsDeleted) throw new RouteAlreadyDeletedException();
        IsDeleted = true;
        UpdateTimestamp();
    }
    public void Restore()
    {
        if (!IsDeleted) throw new RouteNotDeletedException();
        IsDeleted = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}