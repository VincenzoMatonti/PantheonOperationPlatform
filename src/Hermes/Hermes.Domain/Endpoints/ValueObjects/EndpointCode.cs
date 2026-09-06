namespace Hermes.Domain.Endpoints.ValueObjects;

public class EndpointCode
{
    public string Value { get; }

    private EndpointCode(string value)
    {
        Value = value;
    }

    public static EndpointCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Endpoint code cannot be empty.", nameof(value));
        }

        return new EndpointCode(value.Trim());
    }

    public override string ToString() => Value;
}