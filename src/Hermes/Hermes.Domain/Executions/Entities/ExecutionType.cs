using Hermes.Domain.Executions.ValueObjects;

namespace Hermes.Domain.Executions.Entities;

public class ExecutionType
{
    private ExecutionType()
    {
    }

    private ExecutionType(Guid id, ExecutionTypeCode code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public ExecutionTypeCode Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static ExecutionType Create(ExecutionTypeCode code, string name)
    {
        ArgumentNullException.ThrowIfNull(code);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Execution type name cannot be empty.", nameof(name));
        }

        return new ExecutionType(Guid.NewGuid(), code, name.Trim());
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
            throw new ArgumentException("Execution type name cannot be empty.", nameof(name));
        }

        Name = name.Trim();

        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}