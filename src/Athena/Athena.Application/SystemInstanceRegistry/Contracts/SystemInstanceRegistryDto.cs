namespace Athena.Application.SystemInstanceRegistry.Contracts;

public sealed record SystemInstanceRegistryDto(
    Guid Id,
    Guid SystemId,
    Guid WorkloadId,
    string Name,
    string RegistrationStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);

