using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class PostalAddress : IEquatable<PostalAddress>
{
    public const int MaxStreetLength = 200;
    public const int MaxCityLength = 100;
    public const int MaxPostalCodeLength = 20;
    public const int MaxRegionLength = 100;
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Region { get; }
    public CountryCode CountryCode { get; }

    private PostalAddress(string street, string city, string postalCode, string region, CountryCode countryCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Region = region;
        CountryCode = countryCode;
    }

    public static PostalAddress Create(string street, string city, string postalCode, CountryCode countryCode, string? region = null)
    {
        if (string.IsNullOrWhiteSpace(street) || street.Trim().Length > MaxStreetLength || string.IsNullOrWhiteSpace(city) || city.Trim().Length > MaxCityLength ||
        string.IsNullOrWhiteSpace(postalCode) || postalCode.Trim().Length > MaxPostalCodeLength || (region?.Trim().Length > MaxRegionLength) || countryCode == null)
            throw new PostalAddressInvalidException();

        return new PostalAddress(street.Trim(), city.Trim(), postalCode.Trim(), region?.Trim() ?? string.Empty, countryCode);
    }

    public bool Equals(PostalAddress? other)
    {
        return other is not null &&
               string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(PostalCode, other.PostalCode, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Region, other.Region, StringComparison.OrdinalIgnoreCase) &&
               CountryCode.Equals(other.CountryCode);
    }

    public override bool Equals(object? obj) => obj is PostalAddress other && Equals(other);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            StringComparer.OrdinalIgnoreCase.GetHashCode(Street),
            StringComparer.OrdinalIgnoreCase.GetHashCode(City),
            StringComparer.OrdinalIgnoreCase.GetHashCode(PostalCode),
            StringComparer.OrdinalIgnoreCase.GetHashCode(Region),
            CountryCode);
    }
}