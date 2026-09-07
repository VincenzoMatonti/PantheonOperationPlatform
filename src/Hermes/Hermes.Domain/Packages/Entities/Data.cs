using Hermes.Domain.Packages.ValueObjects;

namespace Hermes.Domain.Packages.Entities;

public class Data
{
    private Data() { }

    private Data(Guid id, Guid packageId, DataType type, ContentType contentType, byte[] content, int sequence)
    {
        Id = id;
        PackageId = packageId;
        Type = type;
        ContentType = contentType;
        Content = content;
        Sequence = sequence;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid PackageId { get; private set; }

    public DataType Type { get; private set; } = null!;

    public ContentType ContentType { get; private set; } = null!;

    public byte[] Content { get; private set; } = null!;

    public int Sequence { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Data Create(Guid packageId, DataType type, ContentType contentType, byte[] content, int sequence)
    {
        if (packageId == Guid.Empty)
        {
            throw new ArgumentException("Package ID cannot be empty.", nameof(packageId));
        }

        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(contentType);
        ArgumentNullException.ThrowIfNull(content);

        if (content.Length == 0)
        {
            throw new ArgumentException("Data content cannot be empty.", nameof(content));
        }

        if (sequence < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "Data sequence must be greater than zero.");
        }

        return new Data(Guid.NewGuid(), packageId, type, contentType, content, sequence);
    }

    public void ChangeType(DataType type)
    {
        ArgumentNullException.ThrowIfNull(type);

        Type = type;
        UpdateTimestamp();
    }

    public void ChangeContentType(ContentType contentType)
    {
        ArgumentNullException.ThrowIfNull(contentType);

        ContentType = contentType;
        UpdateTimestamp();
    }

    public void ReplaceContent(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (content.Length == 0)
        {
            throw new ArgumentException("Data content cannot be empty.", nameof(content));
        }

        Content = content;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}