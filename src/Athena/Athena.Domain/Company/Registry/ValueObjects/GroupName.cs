using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class GroupName : IEquatable<GroupName>
{
    public const int MaxLength = 150;

    public string Value { get; }

    private GroupName(string value)
    {
        Value = value;
    }

    public static GroupName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new GroupNameRequiredException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new GroupNameTooLongException(MaxLength);
        return new GroupName(normalized);
    }

    public bool Equals(GroupName? other)
    {
        return other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is GroupName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(GroupName? left, GroupName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(GroupName? left, GroupName? right)
    {
        return !Equals(left, right);
    }
}