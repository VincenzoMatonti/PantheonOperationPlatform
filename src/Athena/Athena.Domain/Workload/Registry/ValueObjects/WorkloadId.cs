using Athena.Domain.Workload.Registry.Exceptions;

namespace Athena.Domain.Workload.Registry.ValueObjects;

public sealed record WorkloadId
{
    public Guid Value { get; }

    private WorkloadId(Guid value)
    {
        Value = value;
    }

    public static WorkloadId New()
    {
        return new WorkloadId(Guid.NewGuid());
    }

    public static WorkloadId Create(Guid value)
    {
        if (value == Guid.Empty) throw new WorkloadIdRequiredException();
        return new WorkloadId(value);
    }

    public override string ToString() => Value.ToString();
}