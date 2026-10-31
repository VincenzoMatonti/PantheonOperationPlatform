using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class RegistrationNumber : IEquatable<RegistrationNumber>
{
    public const int MaxLength = 100;

    public string Value { get; }

    private RegistrationNumber(string value)
    {
        Value = value;
    }

    public static RegistrationNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new RegistrationNumberTooLongException(MaxLength);
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new RegistrationNumberTooLongException(MaxLength);
        return new RegistrationNumber(normalized);
    }

    public bool Equals(RegistrationNumber? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is RegistrationNumber other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;
    public static bool operator ==(RegistrationNumber? left, RegistrationNumber? right) => Equals(left, right);
    public static bool operator !=(RegistrationNumber? left, RegistrationNumber? right) => !Equals(left, right);
}