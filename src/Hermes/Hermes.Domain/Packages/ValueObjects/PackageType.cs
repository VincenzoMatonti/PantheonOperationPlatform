namespace Hermes.Domain.Packages.ValueObjects;

public class PackageType
{
    public string Value { get; }

    private PackageType(string value)
    {
        Value = value;
    }

    public static PackageType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Package type cannot be empty.", nameof(value));
        }

        return new PackageType(value.Trim());
    }

    public override string ToString() => Value;
}