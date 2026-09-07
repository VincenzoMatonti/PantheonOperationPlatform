namespace Hermes.Domain.Packages.ValueObjects;

public class PackageVersion
{
    public string Value { get; }

    private PackageVersion(string value)
    {
        Value = value;
    }

    public static PackageVersion Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Package version cannot be empty.", nameof(value));
        }

        return new PackageVersion(value.Trim());
    }

    public override string ToString() => Value;
}