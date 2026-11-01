using Athena.Domain.Ecosystem.Registry.Entities;

namespace Athena.Application.EcosystemRegistry.Contracts;

public static class EcosystemRegistryDtoMapper
{
    public static EcosystemDto ToDto(Ecosystem ecosystem) => new()
    {
        Id = ecosystem.Id.Value,
        CompanyId = ecosystem.CompanyId.Value,
        Name = ecosystem.Name.Value,
        RegistrationStatus = ecosystem.RegistrationStatus.ToString(),
        AvailabilityStatus = ecosystem.AvailabilityStatus.ToString(),
        CreatedAtUtc = ecosystem.CreatedAtUtc,
        UpdatedAtUtc = ecosystem.UpdatedAtUtc,
        IsDeleted = ecosystem.IsDeleted
    };

    public static List<EcosystemDto> ToDtos(IEnumerable<Ecosystem> ecosystems) => [.. ecosystems.Select(ToDto)];
}
