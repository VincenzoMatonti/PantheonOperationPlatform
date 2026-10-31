using System.Net.Mail;
using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class PecAddress : IEquatable<PecAddress>
{
    public const int MaxLength = 254;

    public string Value { get; }

    private PecAddress(string value)
    {
        Value = value;
    }

    public static PecAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new PecAddressInvalidException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength || !MailAddress.TryCreate(normalized, out var address) || !string.Equals(address.Address, normalized, StringComparison.OrdinalIgnoreCase))
            throw new PecAddressInvalidException();
        return new PecAddress(normalized);
    }

    public bool Equals(PecAddress? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is PecAddress other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;
    public static bool operator ==(PecAddress? left, PecAddress? right) => Equals(left, right);
    public static bool operator !=(PecAddress? left, PecAddress? right) => !Equals(left, right);
}