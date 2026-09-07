namespace Hermes.Domain.Packages.ValueObjects;

public class HeaderKey
{
    public string Value { get; }

    private HeaderKey(string value)
    {
        Value = value;
    }

    public static HeaderKey Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Header key cannot be empty.", nameof(value));
        }

        return new HeaderKey(value.Trim());
    }

    public override string ToString() => Value;
}