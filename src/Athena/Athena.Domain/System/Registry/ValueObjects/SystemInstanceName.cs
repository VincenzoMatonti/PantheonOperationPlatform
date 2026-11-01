using Athena.Domain.System.Registry.Exceptions;

namespace Athena.Domain.System.Registry.ValueObjects;

public class SystemInstanceName : IEquatable<SystemInstanceName>
{
    public const int MaxLength = 150;

    public string Value { get; }

    private SystemInstanceName(string value)
    {
        Value = value;
    }

    public static SystemInstanceName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new SystemInstanceNameRequiredException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new SystemInstanceNameTooLongException(MaxLength);
        return new SystemInstanceName(normalized);
    }

    public bool Equals(SystemInstanceName? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is SystemInstanceName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(SystemInstanceName? left, SystemInstanceName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SystemInstanceName? left, SystemInstanceName? right)
    {
        return !Equals(left, right);
    }
}