
using Athena.Application.SystemRegistry.Dtos;
using Athena.Application.SystemRegistry.Enums;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using DomainRegistrationStatus = Athena.Domain.System.Registry.Enums.SystemRegistrationStatus;
using DomainAvailabilityStatus = Athena.Domain.System.Registry.Enums.SystemAvailabilityStatus;

namespace Athena.Application.SystemRegistry.Queries;

public class SystemRegistryQueryHandler(ISystemRepository systemRepository)
{
    private readonly ISystemRepository _systemRepository = systemRepository;

    public async Task<SystemDto?> GetSystemByIdAsync(GetSystemByIdQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        var system = await _systemRepository.GetByIdAsync(systemId, cancellationToken);
        return system is null ? null : SystemRegistryDtoMapper.ToDto(system);
    }

    public async Task<SystemDto?> GetSystemByNameAsync(GetSystemByNameQuery query, CancellationToken cancellationToken = default)
    {
        var name = SystemName.Create(query.Name);
        var system = await _systemRepository.GetByNameAsync(name, cancellationToken);
        return system is null ? null : SystemRegistryDtoMapper.ToDto(system);
    }

    public async Task<IReadOnlyList<SystemDto>> GetAllSystemsAsync(CancellationToken cancellationToken = default)
    {
        return SystemRegistryDtoMapper.ToDtos(await _systemRepository.GetAllAsync(cancellationToken));
    }

    public async Task<IReadOnlyList<SystemDto>> GetSystemsByRegistrationStatusAsync(
        GetSystemsByRegistrationStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            SystemRegistrationStatusFilter.Registered => DomainRegistrationStatus.Registered,
            SystemRegistrationStatusFilter.Unregistered => DomainRegistrationStatus.Unregistered,
            SystemRegistrationStatusFilter.Disabled => DomainRegistrationStatus.Disabled,
            _ => throw new ArgumentOutOfRangeException(nameof(query), query.Status, "Invalid status value.")
        };

        return SystemRegistryDtoMapper.ToDtos(await _systemRepository.GetByRegistrationStatusAsync(status, cancellationToken));
    }

    public async Task<IReadOnlyList<SystemDto>> GetSystemsByAvailabilityStatusAsync(
        GetSystemsByAvailabilityStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            SystemAvailabilityStatusFilter.Available => DomainAvailabilityStatus.Available,
            SystemAvailabilityStatusFilter.Unavailable => DomainAvailabilityStatus.Unavailable,
            SystemAvailabilityStatusFilter.Unknown => DomainAvailabilityStatus.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(query), query.Status, "Invalid status value.")
        };

        return SystemRegistryDtoMapper.ToDtos(await _systemRepository.GetByAvailabilityStatusAsync(status, cancellationToken));
    }

    public async Task<IReadOnlyList<SystemDto>> GetSystemsByCapabilityAsync(GetSystemsByCapabilityQuery query, CancellationToken cancellationToken = default)
    {
        return SystemRegistryDtoMapper.ToDtos(await _systemRepository.GetByCapabilityNameAsync(query.CapabilityName.Trim(), cancellationToken));
    }

    public async Task<IReadOnlyList<SystemEndpointDto>> GetSystemEndpointsAsync(GetSystemEndpointsQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        return SystemRegistryDtoMapper.ToEndpointDtos(await _systemRepository.GetAllEndpointsAsync(systemId, cancellationToken));
    }

    public async Task<IReadOnlyList<SystemCapabilityDto>> GetSystemCapabilitiesAsync(GetSystemCapabilitiesQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        return SystemRegistryDtoMapper.ToCapabilityDtos(await _systemRepository.GetAllCapabilitiesAsync(systemId, cancellationToken));
    }

    public async Task<IReadOnlyList<SystemDto>> GetSystemsByDeletionStatusAsync(GetSystemsByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        return SystemRegistryDtoMapper.ToDtos(await _systemRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken));
    }

    public async Task<IReadOnlyList<SystemDto>> GetDeletedSystemsAsync(CancellationToken cancellationToken = default)
    {
        return SystemRegistryDtoMapper.ToDtos(await _systemRepository.GetByDeletionStatusAsync(true, cancellationToken));
    }

    public static bool IsUnregistered(SystemDto system) =>
        Enum.TryParse<SystemRegistrationStatusFilter>(system.RegistrationStatus, out var status) &&
        status == SystemRegistrationStatusFilter.Unregistered;
}
