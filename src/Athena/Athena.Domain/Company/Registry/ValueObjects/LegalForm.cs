using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class LegalForm : IEquatable<LegalForm>
{
    public const int MaxLength = 100;

    public string Value { get; }

    private LegalForm(string value)
    {
        Value = value;
    }

    public static LegalForm Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new LegalFormTooLongException(MaxLength);
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new LegalFormTooLongException(MaxLength);
        return new LegalForm(normalized);
    }

    public bool Equals(LegalForm? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is LegalForm other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(LegalForm? left, LegalForm? right) => Equals(left, right);
    public static bool operator !=(LegalForm? left, LegalForm? right) => !Equals(left, right);
}