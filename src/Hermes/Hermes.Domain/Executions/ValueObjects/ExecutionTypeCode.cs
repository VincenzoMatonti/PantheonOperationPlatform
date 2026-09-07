namespace Hermes.Domain.Executions.ValueObjects;

public class ExecutionTypeCode
{
    public string Value { get; }

    private ExecutionTypeCode(string value)
    {
        Value = value;
    }

    public static ExecutionTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Execution type code cannot be empty.", nameof(value));
        }

        return new ExecutionTypeCode(value.Trim());
    }

    public override string ToString() => Value;
}