namespace Hermes.Domain.Packages.ValueObjects;

public class HeaderValue
{
    public string Value { get; }

    private HeaderValue(string value)
    {
        Value = value;
    }

    public static HeaderValue Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Header value cannot be empty.", nameof(value));
        }

        return new HeaderValue(value.Trim());
    }

    public override string ToString() => Value;
}