using Hermes.Domain.Endpoints.Exceptions;

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
        if (string.IsNullOrWhiteSpace(value)) throw new EndpointCodeRequiredException();
        return new EndpointCode(value.Trim());
    }

    public override string ToString() => Value;
}