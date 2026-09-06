namespace Hermes.Domain.Clients.ValueObjects;

public class ClientCode
{
    public string Value { get; }

    private ClientCode(string value)
    {
        Value = value;
    }

    public static ClientCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Client code cannot be empty.", nameof(value));
        }

        return new ClientCode(value.Trim());
    }

    public override string ToString() => Value;
}