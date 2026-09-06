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
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Endpoint type code cannot be empty.", nameof(code));
        }

        return new EndpointType(code.Trim());
    }

    public override string ToString() => Code;
}