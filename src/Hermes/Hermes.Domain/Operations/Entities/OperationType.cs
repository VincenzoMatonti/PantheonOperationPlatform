using Hermes.Domain.Operations.Exceptions;

namespace Hermes.Domain.Operations.Entities;

public class OperationType
{
    private OperationType(Guid id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public static OperationType Create(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new OperationTypeCodeRequiredException();
        if (string.IsNullOrWhiteSpace(name)) throw new OperationTypeNameRequiredException();
        return new OperationType(Guid.NewGuid(), code.Trim(), name.Trim());
    }

    public void Activate()
    {
        if (IsActive) throw new OperationTypeAlreadyActiveException(Id);
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        if (!IsActive) throw new OperationTypeAlreadyInactiveException(Id);
        IsActive = false;
        UpdateTimestamp();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new OperationTypeNameRequiredException();
        Name = name.Trim();
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}