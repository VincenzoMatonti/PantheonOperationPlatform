using Hermes.Domain.Clients.ValueObjects;
using Hermes.Domain.Clients.Exceptions;

namespace Hermes.Domain.Clients.Entities;

public class Client
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
        IsDeleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public ClientCode Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Client Create(ClientCode code, string name)
    {
        if (code is null) throw new ClientCodeRequiredException();
        if (string.IsNullOrWhiteSpace(name)) throw new ClientNameRequiredException();
        return new Client(Guid.NewGuid(), code, name.Trim());
    }

    public void Activate()
    {
        if (IsActive) throw new ClientAlreadyActiveException(Id);
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        if (!IsActive) throw new ClientAlreadyInactiveException(Id);
        IsActive = false;
        UpdateTimestamp();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ClientNameRequiredException();
        Name = name.Trim();
        UpdateTimestamp();
    }

    public void RenameCode(ClientCode code)
    {
        if (code is null) throw new ClientCodeRequiredException();
        Code = code;
        UpdateTimestamp();
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted) throw new ClientAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdateTimestamp();
    }

    public void Restore()
    {
        if (!IsDeleted) throw new ClientNotDeletedException(Id);
        IsDeleted = false;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}