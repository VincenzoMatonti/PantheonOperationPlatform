using Hermes.Domain.Endpoints.Exceptions;

namespace Hermes.Domain.Endpoints.ValueObjects;

public class EndpointType
{
    public string Code { get; }

    private EndpointType(string code)
    {
        Code = code;
    }

    public static EndpointType Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new EndpointTypeRequiredException();
        return new EndpointType(code.Trim());
    }

    public override string ToString() => Code;
}