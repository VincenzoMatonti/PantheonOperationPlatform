using Hermes.Domain.Endpoints.Exceptions;
using Hermes.Domain.Endpoints.ValueObjects;

namespace Hermes.Domain.Endpoints.Entities;

public class Endpoint
{
    private Endpoint(Guid id, EndpointCode code, EndpointType type)
    {
        Id = id;
        Code = code;
        Type = type;
        IsActive = true;
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public EndpointCode Code { get; private set; } = null!;
    public EndpointType Type { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Endpoint Create(EndpointCode code, EndpointType type)
    {
        if (code is null) throw new EndpointCodeRequiredException();
        if (type is null) throw new EndpointTypeRequiredException();
        return new Endpoint(Guid.NewGuid(), code, type);
    }

    public void Rename(EndpointType type)
    {
        if (type is null) throw new EndpointTypeRequiredException(); Type = type; UpdateTimestamp();
    }

    public void RenameCode(EndpointCode code)
    {
        if (code is null) throw new EndpointCodeRequiredException(); Code = code; UpdateTimestamp();
    }

    public void Activate()
    {
        if (IsActive) throw new EndpointAlreadyActiveException(Id);
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        if (!IsActive) throw new EndpointAlreadyInactiveException(Id);
        IsActive = false;
        UpdateTimestamp();
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted) throw new EndpointAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdateTimestamp();
    }

    public void Restore()
    {
        if (!IsDeleted) throw new EndpointNotDeletedException(Id);
        IsDeleted = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}