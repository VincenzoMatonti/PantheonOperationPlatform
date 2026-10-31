using System.Globalization;
using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class CountryCode : IEquatable<CountryCode>
{
    public const int Length = 2;

    public string Value { get; }

    private CountryCode(string value)
    {
        Value = value;
    }

    public static CountryCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new CountryCodeRequiredException();
        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length != Length || !normalized.All(char.IsAsciiLetter) || !CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Select(culture => new RegionInfo(culture.Name).TwoLetterISORegionName)
        .Contains(normalized, StringComparer.OrdinalIgnoreCase)) throw new CountryCodeInvalidException(value);
        return new CountryCode(normalized);
    }

    public bool Equals(CountryCode? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj) => obj is CountryCode other && Equals(other);
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(CountryCode? left, CountryCode? right) => Equals(left, right);
    public static bool operator !=(CountryCode? left, CountryCode? right) => !Equals(left, right);
}