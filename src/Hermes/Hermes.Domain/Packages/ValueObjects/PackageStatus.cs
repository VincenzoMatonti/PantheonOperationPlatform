namespace Hermes.Domain.Packages.ValueObjects;

public enum PackageStatus
{
    Created = 1,
    Ready = 2,
    Processing = 3,
    Completed = 4,
    Failed = 5,
    Cancelled = 6
}