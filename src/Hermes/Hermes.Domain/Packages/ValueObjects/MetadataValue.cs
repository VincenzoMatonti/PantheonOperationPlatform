namespace Hermes.Domain.Packages.ValueObjects;

public class MetadataValue
{
    public string Value { get; }

    private MetadataValue(string value)
    {
        Value = value;
    }

    public static MetadataValue Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Metadata value cannot be empty.", nameof(value));
        }

        return new MetadataValue(value.Trim());
    }

    public override string ToString() => Value;
}