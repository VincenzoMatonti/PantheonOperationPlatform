namespace Athena.Domain.Orchestration.Management.ValueObjects;

public class OrchestrationSystemInstanceId : IEquatable<OrchestrationSystemInstanceId>
{
    public Guid Value { get; }

    private OrchestrationSystemInstanceId(Guid value)
    {
        Value = value;
    }

    public static OrchestrationSystemInstanceId New() => new(Guid.NewGuid());

    public static OrchestrationSystemInstanceId Create(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("Orchestration-system ID cannot be empty.", nameof(value));

        return new OrchestrationSystemInstanceId(value);
    }

    public bool Equals(OrchestrationSystemInstanceId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is OrchestrationSystemInstanceId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(OrchestrationSystemInstanceId? left, OrchestrationSystemInstanceId? right) => Equals(left, right);
    public static bool operator !=(OrchestrationSystemInstanceId? left, OrchestrationSystemInstanceId? right) => !Equals(left, right);
}