using Athena.Domain.Block.Registry.Exceptions;

namespace Athena.Domain.Block.Registry.ValueObjects;

public class BlockName : IEquatable<BlockName>
{
    public const int MaxLength = 150;

    public string Value { get; }

    private BlockName(string value)
    {
        Value = value;
    }

    public static BlockName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new BlockNameRequiredException();
        var normalized = value.Trim();
        if (normalized.Length > MaxLength) throw new BlockNameTooLongException(MaxLength);
        return new BlockName(normalized);
    }

    public bool Equals(BlockName? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is BlockName other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public override string ToString() => Value;
    public static bool operator ==(BlockName? left, BlockName? right) => Equals(left, right);
    public static bool operator !=(BlockName? left, BlockName? right) => !Equals(left, right);
}