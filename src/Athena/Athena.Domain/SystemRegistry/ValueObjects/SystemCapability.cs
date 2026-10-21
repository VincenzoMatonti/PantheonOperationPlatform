using Athena.Domain.SystemRegistry.Exceptions;

namespace Athena.Domain.SystemRegistry.ValueObjects;

public class SystemCapability : IEquatable<SystemCapability>
{
    public string Name { get; }
    public string Description { get; }
    private SystemCapability(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static SystemCapability Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new SystemCapabilityRequiredException();
        var normalizedName = name.Trim();
        var normalizedDescription = description?.Trim() ?? string.Empty;

        return new SystemCapability(normalizedName, normalizedDescription);
    }

    public bool Equals(SystemCapability? other)
    {
        return other is not null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is SystemCapability other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }

    public override string ToString()
    {
        return Name;
    }
}