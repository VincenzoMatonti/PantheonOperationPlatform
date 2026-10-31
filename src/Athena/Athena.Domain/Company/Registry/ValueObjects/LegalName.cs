using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class LegalName : IEquatable<LegalName>
{
    public const int MaxLength = 200;

    public string Value { get; }

    private LegalName(string value)
    {
        Value = value;
    }

    public static LegalName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new LegalNameRequiredException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new LegalNameTooLongException(MaxLength);
        return new LegalName(normalized);
    }

    public bool Equals(LegalName? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => obj is LegalName other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(LegalName? left, LegalName? right) => Equals(left, right);
    public static bool operator !=(LegalName? left, LegalName? right) => !Equals(left, right);
}