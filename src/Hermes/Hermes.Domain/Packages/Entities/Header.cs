using Hermes.Domain.Packages.ValueObjects;

namespace Hermes.Domain.Packages.Entities;

public class Header
{
    private Header() { }

    private Header(Guid id, Guid packageId, HeaderKey key, HeaderValue value)
    {
        Id = id;
        PackageId = packageId;
        Key = key;
        Value = value;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid PackageId { get; private set; }

    public HeaderKey Key { get; private set; } = null!;

    public HeaderValue Value { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Header Create(Guid packageId, HeaderKey key, HeaderValue value)
    {
        if (packageId == Guid.Empty)
        {
            throw new ArgumentException("Package ID cannot be empty.", nameof(packageId));
        }

        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        return new Header(Guid.NewGuid(), packageId, key, value);
    }

    public void ChangeKey(HeaderKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        Key = key;
        UpdateTimestamp();
    }

    public void ChangeValue(HeaderValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}