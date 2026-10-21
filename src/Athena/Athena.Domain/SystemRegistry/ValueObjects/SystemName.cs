using Athena.Domain.SystemRegistry.Exceptions;

namespace Athena.Domain.SystemRegistry.ValueObjects;

public class SystemName : IEquatable<SystemName>
{
    public const int MaxLength = 100;

    public string Value { get; }

    private SystemName(string value)
    {
        Value = value;
    }

    public static SystemName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new SystemNameRequiredException();

        var normalized = value.Trim();

        if (normalized.Length > MaxLength)
            throw new SystemNameTooLongException(MaxLength);

        return new SystemName(normalized);
    }

    public bool Equals(SystemName? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is SystemName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(SystemName? left, SystemName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SystemName? left, SystemName? right)
    {
        return !Equals(left, right);
    }
}