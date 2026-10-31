using Athena.Domain.Ecosystem.Registry.Exceptions;

namespace Athena.Domain.Ecosystem.Registry.ValueObjects;

public class EcosystemName : IEquatable<EcosystemName>
{
    public const int MaxLength = 150;

    public string Value { get; }

    private EcosystemName(string value)
    {
        Value = value;
    }

    public static EcosystemName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new EcosystemNameRequiredException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new EcosystemNameTooLongException(MaxLength);
        return new EcosystemName(normalized);
    }

    public bool Equals(EcosystemName? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is EcosystemName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(EcosystemName? left, EcosystemName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EcosystemName? left, EcosystemName? right)
    {
        return !Equals(left, right);
    }
}