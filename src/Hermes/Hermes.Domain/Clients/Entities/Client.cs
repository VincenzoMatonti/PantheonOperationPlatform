using Hermes.Domain.Clients.ValueObjects;

namespace Hermes.Domain.Clients.Entities;

public sealed class Client
{
    private Client()
    {
    }

    private Client(Guid id, ClientCode code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public ClientCode Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Client Create(ClientCode code, string name)
    {
        ArgumentNullException.ThrowIfNull(code);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Client name cannot be empty.", nameof(name));
        }

        return new Client(Guid.NewGuid(), code, name.Trim());
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

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Client name cannot be empty.", nameof(name));
        }

        Name = name.Trim();
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}