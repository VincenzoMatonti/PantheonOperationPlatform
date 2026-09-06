using Hermes.Domain.Endpoints.ValueObjects;

namespace Hermes.Domain.Endpoints.Entities;

public class Endpoint
{
    private Endpoint()
    {
    }

    private Endpoint(Guid id, EndpointCode code, EndpointType type)
    {
        Id = id;
        Code = code;
        Type = type;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public EndpointCode Code { get; private set; } = null!;

    public EndpointType Type { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Endpoint Create(EndpointCode code, EndpointType type)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(type);

        return new Endpoint(Guid.NewGuid(), code, type);
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