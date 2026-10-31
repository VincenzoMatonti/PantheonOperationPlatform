using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class CompanyName : IEquatable<CompanyName>
{
    public const int MaxLength = 150;

    public string Value { get; }

    private CompanyName(string value)
    {
        Value = value;
    }

    public static CompanyName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new CompanyNameRequiredException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new CompanyNameTooLongException(MaxLength);
        return new CompanyName(normalized);
    }

    public bool Equals(CompanyName? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is CompanyName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(CompanyName? left, CompanyName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CompanyName? left, CompanyName? right)
    {
        return !Equals(left, right);
    }
}