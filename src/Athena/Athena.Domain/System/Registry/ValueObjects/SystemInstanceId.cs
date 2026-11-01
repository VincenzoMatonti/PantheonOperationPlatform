using Athena.Domain.System.Registry.Exceptions;

namespace Athena.Domain.System.Registry.ValueObjects;

public class SystemInstanceId : IEquatable<SystemInstanceId>
{
    public Guid Value { get; }

    private SystemInstanceId(Guid value)
    {
        Value = value;
    }

    public static SystemInstanceId New()
    {
        return new SystemInstanceId(Guid.NewGuid());
    }

    public static SystemInstanceId Create(Guid value)
    {
        if (value == Guid.Empty) throw new SystemInstanceIdRequiredException();
        return new SystemInstanceId(value);
    }

    public bool Equals(SystemInstanceId? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is SystemInstanceId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(SystemInstanceId? left, SystemInstanceId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SystemInstanceId? left, SystemInstanceId? right)
    {
        return !Equals(left, right);
    }
}