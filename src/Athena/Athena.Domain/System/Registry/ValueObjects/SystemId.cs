using Athena.Domain.System.Registry.Exceptions;

namespace Athena.Domain.System.Registry.ValueObjects;

public class SystemId : IEquatable<SystemId>
{
    public Guid Value { get; }

    private SystemId(Guid value)
    {
        Value = value;
    }

    public static SystemId New()
    {
        return new SystemId(Guid.NewGuid());
    }

    public static SystemId Create(Guid value)
    {
        if (value == Guid.Empty) throw new SystemIdRequiredException();
        return new SystemId(value);
    }

    public bool Equals(SystemId? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is SystemId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(SystemId? left, SystemId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SystemId? left, SystemId? right)
    {
        return !Equals(left, right);
    }
}