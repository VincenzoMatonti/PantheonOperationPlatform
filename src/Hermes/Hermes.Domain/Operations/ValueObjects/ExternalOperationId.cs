using Hermes.Domain.Operations.Exceptions;

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
        if (string.IsNullOrWhiteSpace(value)) throw new OperationExternalIdRequiredException();
        return new ExternalOperationId(value.Trim());
    }

    public override string ToString() => Value;
}