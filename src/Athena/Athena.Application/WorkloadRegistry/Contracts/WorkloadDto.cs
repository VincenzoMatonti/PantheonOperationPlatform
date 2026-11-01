namespace Athena.Application.WorkloadRegistry.Contracts;

public sealed record WorkloadDto(
    Guid Id,
    Guid BlockId,
    string Name,
    string RegistrationStatus,
    string AvailabilityStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);