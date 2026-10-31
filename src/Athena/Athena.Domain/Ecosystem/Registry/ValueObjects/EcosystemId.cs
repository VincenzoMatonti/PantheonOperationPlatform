using Athena.Domain.Ecosystem.Registry.Exceptions;

namespace Athena.Domain.Ecosystem.Registry.ValueObjects;

public class EcosystemId : IEquatable<EcosystemId>
{
    public Guid Value { get; }

    private EcosystemId(Guid value)
    {
        Value = value;
    }

    public static EcosystemId New()
    {
        return new EcosystemId(Guid.NewGuid());
    }

    public static EcosystemId Create(Guid value)
    {
        if (value == Guid.Empty) throw new EcosystemIdRequiredException();
        return new EcosystemId(value);
    }

    public bool Equals(EcosystemId? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is EcosystemId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(EcosystemId? left, EcosystemId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EcosystemId? left, EcosystemId? right)
    {
        return !Equals(left, right);
    }
}