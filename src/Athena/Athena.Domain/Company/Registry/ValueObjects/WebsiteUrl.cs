using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class WebsiteUrl : IEquatable<WebsiteUrl>
{
    public const int MaxLength = 2048;

    public string Value { get; }

    private WebsiteUrl(string value)
    {
        Value = value;
    }

    public static WebsiteUrl Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new WebsiteUrlInvalidException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength || !Uri.TryCreate(normalized, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new WebsiteUrlInvalidException();
        return new WebsiteUrl(uri.ToString());
    }

    public bool Equals(WebsiteUrl? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is WebsiteUrl other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;
    public static bool operator ==(WebsiteUrl? left, WebsiteUrl? right) => Equals(left, right);
    public static bool operator !=(WebsiteUrl? left, WebsiteUrl? right) => !Equals(left, right);
}