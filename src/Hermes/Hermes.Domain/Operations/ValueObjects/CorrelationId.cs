using Hermes.Domain.Operations.Exceptions;

namespace Hermes.Domain.Operations.ValueObjects;

public class CorrelationId
{
    public Guid Value { get; }

    private CorrelationId(Guid value)
    {
        Value = value;
    }

    public static CorrelationId Create()
    {
        return new CorrelationId(Guid.NewGuid());
    }

    public static CorrelationId From(Guid value)
    {
        if (value == Guid.Empty) throw new OperationCorrelationIdRequiredException();                  
        return new CorrelationId(value);
    }

    public override string ToString() => Value.ToString();
}