namespace Athena.Domain.OrchestrationManagement.ValueObjects;

public class OrchestrationSystemId : IEquatable<OrchestrationSystemId>
{
    public Guid Value { get; }

    private OrchestrationSystemId(Guid value)
    {
        Value = value;
    }

    public static OrchestrationSystemId New() => new(Guid.NewGuid());

    public static OrchestrationSystemId Create(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("Orchestration-system ID cannot be empty.", nameof(value));

        return new OrchestrationSystemId(value);
    }

    public bool Equals(OrchestrationSystemId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is OrchestrationSystemId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(OrchestrationSystemId? left, OrchestrationSystemId? right) => Equals(left, right);
    public static bool operator !=(OrchestrationSystemId? left, OrchestrationSystemId? right) => !Equals(left, right);
}