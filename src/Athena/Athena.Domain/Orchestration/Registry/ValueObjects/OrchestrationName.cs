using Athena.Domain.Orchestration.Registry.Exceptions;

namespace Athena.Domain.Orchestration.Registry.ValueObjects;

public class OrchestrationName
{
    public string Value { get; }

    private OrchestrationName(string value)
    {
        Value = value;
    }

    public static OrchestrationName Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new OrchestrationNameRequiredException();
        return new OrchestrationName(value.Trim());
    }

    public override string ToString() => Value;
}
