using Athena.Domain.Company.Registry.Exceptions;

namespace Athena.Domain.Company.Registry.ValueObjects;

public class CompanyId : IEquatable<CompanyId>
{
    public Guid Value { get; }

    private CompanyId(Guid value)
    {
        Value = value;
    }

    public static CompanyId New()
    {
        return new CompanyId(Guid.NewGuid());
    }

    public static CompanyId Create(Guid value)
    {
        if (value == Guid.Empty) throw new CompanyIdRequiredException();
        return new CompanyId(value);
    }

    public bool Equals(CompanyId? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is CompanyId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(CompanyId? left, CompanyId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CompanyId? left, CompanyId? right)
    {
        return !Equals(left, right);
    }
}