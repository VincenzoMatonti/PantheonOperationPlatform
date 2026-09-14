using Hermes.Domain.Operations.Exceptions;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Domain.Operations.Entities;

public class OperationType
{
    private OperationType(Guid id, OperationTypeCode code, string name)
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
    public OperationTypeCode Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static OperationType Create(OperationTypeCode code, string name)
    {
        if (code is null) throw new OperationTypeCodeRequiredException();
        if (string.IsNullOrWhiteSpace(name)) throw new OperationTypeNameRequiredException();

        return new OperationType(Guid.NewGuid(), code, name.Trim());
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

    public void MarkAsDeleted()
    {
        if (IsDeleted) throw new OperationTypeAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdateTimestamp();
    }

    public void Restore()
    {
        if (!IsDeleted) throw new OperationTypeNotDeletedException(Id);
        IsDeleted = false;
        UpdateTimestamp();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new OperationTypeNameRequiredException();
        Name = name.Trim();
        UpdateTimestamp();
    }

    public void RenameCode(OperationTypeCode code)
    {
        if (code is null) throw new OperationTypeCodeRequiredException();
        Code = code;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}