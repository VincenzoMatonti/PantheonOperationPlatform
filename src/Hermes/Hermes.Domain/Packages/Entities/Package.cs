using Hermes.Domain.Packages.ValueObjects;

namespace Hermes.Domain.Packages.Entities;

public class Package
{
    private Package() { }

    private Package(Guid id, Guid operationId, PackageType type, PackageStatus status, ContentType contentType, PackageVersion version, int sequence)
    {
        Id = id;
        OperationId = operationId;
        Type = type;
        Status = status;
        ContentType = contentType;
        Version = version;
        Sequence = sequence;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public Guid OperationId { get; private set; }

    public PackageType Type { get; private set; } = null!;

    public PackageStatus Status { get; private set; }

    public ContentType ContentType { get; private set; } = null!;

    public PackageVersion Version { get; private set; } = null!;

    public int Sequence { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Package Create(Guid operationId, PackageType type, ContentType contentType, PackageVersion version, int sequence)
    {
        if (operationId == Guid.Empty)
        {
            throw new ArgumentException("Operation ID cannot be empty.", nameof(operationId));
        }

        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(contentType);
        ArgumentNullException.ThrowIfNull(version);

        if (sequence < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "Package sequence must be greater than zero.");
        }

        return new Package(Guid.NewGuid(), operationId, type, PackageStatus.Created, contentType, version, sequence);
    }

    public void SetReady()
    {
        if (Status != PackageStatus.Created)
        {
            throw new InvalidOperationException("Only created packages can be marked as ready.");
        }

        Status = PackageStatus.Ready;
        UpdateTimestamp();
    }

    public void StartProcessing()
    {
        if (Status != PackageStatus.Ready)
        {
            throw new InvalidOperationException("Only ready packages can start processing.");
        }

        Status = PackageStatus.Processing;
        UpdateTimestamp();
    }

    public void Complete()
    {
        if (Status != PackageStatus.Processing)
        {
            throw new InvalidOperationException("Only processing packages can be completed.");
        }
        Status = PackageStatus.Completed;
        UpdateTimestamp();
    }

    public void Fail()
    {
        if (Status != PackageStatus.Processing)
        {
            throw new InvalidOperationException("Only processing packages can be failed.");
        }

        Status = PackageStatus.Failed;
        UpdateTimestamp();
    }

    public void Cancel()
    {
        if (Status != PackageStatus.Created && Status != PackageStatus.Ready && Status != PackageStatus.Processing)
        {
            throw new InvalidOperationException("Only created, ready or processing packages can be cancelled.");
        }

        Status = PackageStatus.Cancelled;
        UpdateTimestamp();
    }

    public void ChangeType(PackageType type)
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

    public void ChangeVersion(PackageVersion version)
    {
        ArgumentNullException.ThrowIfNull(version);

        Version = version;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}