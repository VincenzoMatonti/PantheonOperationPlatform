using System.Net.Mail;
using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class EmailAddress : IEquatable<EmailAddress>
{
    public const int MaxLength = 254;

    public string Value { get; }

    protected EmailAddress(string value)
    {
        Value = value;
    }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new EmailAddressInvalidException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength || !MailAddress.TryCreate(normalized, out var address) || !string.Equals(address.Address, normalized, StringComparison.OrdinalIgnoreCase))
            throw new EmailAddressInvalidException();
        return new EmailAddress(normalized);
    }

    public bool Equals(EmailAddress? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is EmailAddress other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;

    public static bool operator ==(EmailAddress? left, EmailAddress? right) => Equals(left, right);
    public static bool operator !=(EmailAddress? left, EmailAddress? right) => !Equals(left, right);
}