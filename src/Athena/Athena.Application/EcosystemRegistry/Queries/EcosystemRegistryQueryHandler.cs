using Athena.Application.EcosystemRegistry.Contracts;
using Athena.Application.EcosystemRegistry.Enums;
using Athena.Domain.Ecosystem.Registry.Enums;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.EcosystemRegistry.Queries;

public sealed class EcosystemRegistryQueryHandler(IEcosystemRepository ecosystemRepository)
{
    private readonly IEcosystemRepository _ecosystemRepository = ecosystemRepository;

    public async Task<EcosystemDto?> GetByIdAsync(GetEcosystemByIdQuery query, CancellationToken cancellationToken = default)
    {
        var ecosystem = await _ecosystemRepository.GetByIdAsync(EcosystemId.Create(query.EcosystemId), cancellationToken);
        return ecosystem is null ? null : EcosystemRegistryDtoMapper.ToDto(ecosystem);
    }

    public async Task<EcosystemDto?> GetByNameAsync(GetEcosystemByNameQuery query, CancellationToken cancellationToken = default)
    {
        var ecosystem = await _ecosystemRepository.GetByNameAsync(EcosystemName.Create(query.Name), cancellationToken);
        return ecosystem is null ? null : EcosystemRegistryDtoMapper.ToDto(ecosystem);
    }

    public async Task<IReadOnlyList<EcosystemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var ecosystems = await _ecosystemRepository.GetAllAsync(cancellationToken);
        return EcosystemRegistryDtoMapper.ToDtos(ecosystems);
    }

    public async Task<IReadOnlyList<EcosystemDto>> GetByCompanyIdAsync(GetEcosystemsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var ecosystems = await _ecosystemRepository.GetByCompanyIdAsync(CompanyId.Create(query.CompanyId), cancellationToken);
        return EcosystemRegistryDtoMapper.ToDtos(ecosystems);
    }

    public async Task<IReadOnlyList<EcosystemDto>> GetByRegistrationStatusAsync(GetEcosystemsByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            EcosystemRegistrationStatusFilter.Registered => EcosystemRegistrationStatus.Registered,
            EcosystemRegistrationStatusFilter.Unregistered => EcosystemRegistrationStatus.Unregistered,
            EcosystemRegistrationStatusFilter.Disabled => EcosystemRegistrationStatus.Disabled,
            _ => (EcosystemRegistrationStatus?)null
        };

        if (status is null) return [];
        var ecosystems = await _ecosystemRepository.GetByRegistrationStatusAsync(status.Value, cancellationToken);
        return EcosystemRegistryDtoMapper.ToDtos(ecosystems);
    }

    public async Task<IReadOnlyList<EcosystemDto>> GetByAvailabilityStatusAsync(GetEcosystemsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            EcosystemAvailabilityStatusFilter.Available => EcosystemAvailabilityStatus.Available,
            EcosystemAvailabilityStatusFilter.Unavailable => EcosystemAvailabilityStatus.Unavailable,
            EcosystemAvailabilityStatusFilter.Unknown => EcosystemAvailabilityStatus.Unknown,
            EcosystemAvailabilityStatusFilter.Error => EcosystemAvailabilityStatus.Error,
            _ => (EcosystemAvailabilityStatus?)null
        };

        if (status is null) return [];
        var ecosystems = await _ecosystemRepository.GetByAvailabilityStatusAsync(status.Value, cancellationToken);
        return EcosystemRegistryDtoMapper.ToDtos(ecosystems);
    }

    public async Task<IReadOnlyList<EcosystemDto>> GetByDeletionStatusAsync(GetEcosystemsByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var ecosystems = await _ecosystemRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return EcosystemRegistryDtoMapper.ToDtos(ecosystems);
    }

    public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default) => _ecosystemRepository.ExistsByNameAsync(EcosystemName.Create(name), cancellationToken);
}