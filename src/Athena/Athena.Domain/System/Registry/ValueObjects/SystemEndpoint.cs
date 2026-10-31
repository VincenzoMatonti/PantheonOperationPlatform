using Athena.Domain.System.Registry.Exceptions;

namespace Athena.Domain.System.Registry.ValueObjects;

public class SystemEndpoint : IEquatable<SystemEndpoint>
{
    public string Name { get; }

    public Uri BaseAddress { get; }

    private SystemEndpoint(string name, Uri baseAddress)
    {
        Name = name;
        BaseAddress = baseAddress;
    }

    public static SystemEndpoint Create(string name, string baseAddress)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(baseAddress))
        {
            throw new SystemEndpointRequiredException();
        }

        var normalizedName = name.Trim();
        var normalizedAddress = baseAddress.Trim();
        if (!Uri.TryCreate(normalizedAddress, UriKind.Absolute, out var uri) ||
           (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new SystemEndpointInvalidException(normalizedAddress);
        }

        return new SystemEndpoint(normalizedName, uri);
    }

    public bool Equals(SystemEndpoint? other)
    {
        return other is not null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is SystemEndpoint other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }

    public override string ToString()
    {
        return $"{Name}: {BaseAddress}";
    }
}