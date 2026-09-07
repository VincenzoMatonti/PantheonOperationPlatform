namespace Hermes.Domain.Packages.ValueObjects;

public class ContentType
{
    public string Value { get; }

    private ContentType(string value)
    {
        Value = value;
    }

    public static ContentType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Content type cannot be empty.", nameof(value));
        }

        return new ContentType(value.Trim());
    }

    public override string ToString() => Value;
}