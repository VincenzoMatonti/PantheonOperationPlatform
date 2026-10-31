using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class GroupId : IEquatable<GroupId>
{
    public Guid Value { get; }

    private GroupId(Guid value)
    {
        Value = value;
    }

    public static GroupId New()
    {
        return new GroupId(Guid.NewGuid());
    }

    public static GroupId Create(Guid value)
    {
        if (value == Guid.Empty) throw new GroupIdRequiredException();
        return new GroupId(value);
    }

    public bool Equals(GroupId? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is GroupId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(GroupId? left, GroupId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(GroupId? left, GroupId? right)
    {
        return !Equals(left, right);
    }
}