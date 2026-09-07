namespace Hermes.Domain.Packages.ValueObjects;

public class DataType
{
    public string Value { get; }

    private DataType(string value)
    {
        Value = value;
    }

    public static DataType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Data type cannot be empty.", nameof(value));
        }

        return new DataType(value.Trim());
    }

    public override string ToString() => Value;
}