using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class TaxCode : IEquatable<TaxCode>
{
    public const int MaxLength = 50;

    public string Value { get; }

    private TaxCode(string value)
    {
        Value = value;
    }

    public static TaxCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new TaxCodeInvalidException();
        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length > MaxLength) throw new TaxCodeInvalidException();
        return new TaxCode(normalized);
    }

    public bool Equals(TaxCode? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => obj is TaxCode other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(TaxCode? left, TaxCode? right) => Equals(left, right);
    public static bool operator !=(TaxCode? left, TaxCode? right) => !Equals(left, right);
}