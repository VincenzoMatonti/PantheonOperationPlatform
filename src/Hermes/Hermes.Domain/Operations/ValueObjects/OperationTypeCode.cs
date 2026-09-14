using Hermes.Domain.Operations.Exceptions;

namespace Hermes.Domain.Operations.ValueObjects;

public class OperationTypeCode
{
    public string Value { get; }

    private OperationTypeCode(string value)
    {
        Value = value;
    }

    public static OperationTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new OperationTypeCodeRequiredException();
        return new OperationTypeCode(value.Trim());
    }

    public override string ToString() => Value;
}