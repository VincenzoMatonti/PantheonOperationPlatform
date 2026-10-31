using Athena.Domain.Workload.Registry.Exceptions;

namespace Athena.Domain.Workload.Registry.ValueObjects;

public sealed record WorkloadName
{
    public const int MaxLength = 150;

    public string Value { get; }

    private WorkloadName(string value)
    {
        Value = value;
    }

    public static WorkloadName Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new WorkloadNameRequiredException();
        var normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength) throw new WorkloadNameTooLongException(MaxLength);
        return new WorkloadName(normalizedValue);
    }

    public override string ToString() => Value;
}