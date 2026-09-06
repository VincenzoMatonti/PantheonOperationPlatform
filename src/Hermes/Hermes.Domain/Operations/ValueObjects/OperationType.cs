namespace Hermes.Domain.Operations.ValueObjects;

public class OperationType
{
    public string Code { get; }

    private OperationType(string code)
    {
        Code = code;
    }

    public static OperationType Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Operation type code cannot be empty.", nameof(code));
        }
            
        return new OperationType(code.Trim());
    }

    public override string ToString() => Code;
}