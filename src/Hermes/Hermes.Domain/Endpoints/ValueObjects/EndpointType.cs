using Hermes.Domain.Endpoints.Exceptions;

namespace Hermes.Domain.Endpoints.ValueObjects;

public class EndpointType
{
    public string Value { get; }

    private EndpointType(string value)
    {
        Value = value;
    }

    public static EndpointType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new EndpointTypeRequiredException();
        return new EndpointType(value.Trim());
    }

    public override string ToString() => Value;
}