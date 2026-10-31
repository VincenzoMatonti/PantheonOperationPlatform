using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class PhoneNumber : IEquatable<PhoneNumber>
{
    public const int MaxLength = 30;

    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new PhoneNumberInvalidException();
        var normalized = value.Trim();
        var digits = normalized.Count(char.IsAsciiDigit);
        if (normalized.Length > MaxLength || digits < 7 || digits > 15 || normalized.Any(character => !char.IsAsciiDigit(character)
        && character is not '+' and not '-' and not '(' and not ')' and not ' ')) throw new PhoneNumberInvalidException();
        return new PhoneNumber(normalized);
    }

    public bool Equals(PhoneNumber? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is PhoneNumber other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(PhoneNumber? left, PhoneNumber? right) => Equals(left, right);
    public static bool operator !=(PhoneNumber? left, PhoneNumber? right) => !Equals(left, right);
}