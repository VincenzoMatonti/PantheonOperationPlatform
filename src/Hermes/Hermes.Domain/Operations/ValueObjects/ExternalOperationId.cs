namespace Hermes.Domain.Operations.ValueObjects;

public class ExternalOperationId
{
    public string Value { get; }

    private ExternalOperationId(string value)
    {
        Value = value;
    }

    public static ExternalOperationId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("External operation ID cannot be empty.", nameof(value));
        }

        return new ExternalOperationId(value.Trim());
    }

    public override string ToString() => Value;
}