using Athena.Domain.OrchestrationRegistry.Exceptions;

namespace Athena.Domain.OrchestrationRegistry.ValueObjects;

public class OrchestrationId
{
    public Guid Value { get; }

    private OrchestrationId(Guid value)
    {
        Value = value;
    }

    public static OrchestrationId Create(Guid value)
    {
        if (value == Guid.Empty) throw new OrchestrationIdInvalidException();
        return new OrchestrationId(value);
    }

    public override string ToString() => Value.ToString();
}
