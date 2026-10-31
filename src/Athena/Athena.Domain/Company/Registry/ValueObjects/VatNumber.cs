using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class VatNumber : IEquatable<VatNumber>
{
    public const int MaxLength = 30;

    public string Value { get; }

    private VatNumber(string value)
    {
        Value = value;
    }

    public static VatNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new VatNumberInvalidException();
        var normalized = value.Trim().Replace(" ", "").ToUpperInvariant();
        if (normalized.Length > MaxLength || !normalized.All(char.IsAsciiLetterOrDigit)) throw new VatNumberInvalidException();
        return new VatNumber(normalized);
    }

    public bool Equals(VatNumber? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => obj is VatNumber other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(VatNumber? left, VatNumber? right) => Equals(left, right);
    public static bool operator !=(VatNumber? left, VatNumber? right) => !Equals(left, right);
}