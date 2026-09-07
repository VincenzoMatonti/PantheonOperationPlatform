using Hermes.Domain.Packages.ValueObjects;

namespace Hermes.Domain.Packages.Entities;

public class Metadata
{
    private Metadata() { }

    private Metadata(Guid id, Guid packageId, Guid? dataId, MetadataKey key, MetadataValue value)
    {
        Id = id;
        PackageId = packageId;
        DataId = dataId;
        Key = key;
        Value = value;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid PackageId { get; private set; }

    public Guid? DataId { get; private set; }

    public MetadataKey Key { get; private set; } = null!;

    public MetadataValue Value { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Metadata CreateForPackage(Guid packageId, MetadataKey key, MetadataValue value)
    {
        if (packageId == Guid.Empty)
        {
            throw new ArgumentException("Package ID cannot be empty.", nameof(packageId));
        }

        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        return new Metadata(Guid.NewGuid(), packageId, null, key, value);
    }

    public static Metadata CreateForData(Guid packageId, Guid dataId, MetadataKey key, MetadataValue value)
    {
        if (packageId == Guid.Empty)
        {
            throw new ArgumentException("Package ID cannot be empty.", nameof(packageId));
        }

        if (dataId == Guid.Empty)
        {
            throw new ArgumentException("Data ID cannot be empty.", nameof(dataId));
        }

        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        return new Metadata(Guid.NewGuid(), packageId, dataId, key, value);
    }

    public bool BelongsToPackage() => DataId is null;

    public bool BelongsToData() => DataId is not null;

    public void ChangeValue(MetadataValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
        UpdateTimestamp();
    }

    public void ChangeKey(MetadataKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        Key = key;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}