namespace Hermes.Domain.Packages.ValueObjects;

public class MetadataKey
{
    public string Value { get; }

    private MetadataKey(string value)
    {
        Value = value;
    }

    public static MetadataKey Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Metadata key cannot be empty.", nameof(value));
        }

        return new MetadataKey(value.Trim());
    }

    public override string ToString() => Value;
}