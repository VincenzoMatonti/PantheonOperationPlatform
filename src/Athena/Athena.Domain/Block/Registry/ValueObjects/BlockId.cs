namespace Athena.Domain.Block.Registry.ValueObjects;

public class BlockId : IEquatable<BlockId>
{
    public Guid Value { get; }

    private BlockId(Guid value)
    {
        Value = value;
    }

    public static BlockId New() => new(Guid.NewGuid());

    public static BlockId Create(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("Block ID cannot be empty.", nameof(value));
        return new BlockId(value);
    }

    public bool Equals(BlockId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is BlockId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(BlockId? left, BlockId? right) => Equals(left, right);
    public static bool operator !=(BlockId? left, BlockId? right) => !Equals(left, right);
}