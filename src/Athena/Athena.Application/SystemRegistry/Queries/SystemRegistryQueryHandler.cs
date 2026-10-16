
using Athena.Application.SystemRegistry.Dtos;
using Athena.Application.SystemRegistry.Enums;
using Athena.Domain.SystemRegistry.Repositories;
using Athena.Domain.SystemRegistry.ValueObjects;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;
using DomainRegistrationStatus = Athena.Domain.SystemRegistry.Enums.SystemRegistrationStatus;
using DomainAvailabilityStatus = Athena.Domain.SystemRegistry.Enums.SystemAvailabilityStatus;

namespace Athena.Application.SystemRegistry.Queries;

public class SystemRegistryQueryHandler(ISystemRepository systemRepository)
{
    private readonly ISystemRepository _systemRepository = systemRepository;

    public static SystemDto ConvertSystemEntitiesToDto(RegisteredSystem system)
    {
        return new SystemDto
        {
            Id = system.Id.Value,
            Name = system.Name.Value,
            RegistrationStatus = system.RegistrationStatus.ToString(),
            AvailabilityStatus = system.AvailabilityStatus.ToString(),
            Endpoints = [.. system.Endpoints.Select(ConvertSystemEndpointEntitiesToDto)],
            Capabilities = [.. system.Capabilities.Select(ConvertSystemCapabilityEntitiesToDto)],
            CreatedAtUtc = system.CreatedAtUtc,
            UpdatedAtUtc = system.UpdatedAtUtc,
            IsDeleted = system.IsDeleted
        };
    }

    public static List<SystemDto> ConvertSystemEntitiesToDto(IReadOnlyList<RegisteredSystem> systems)
    {
        return [.. systems.Select(ConvertSystemEntitiesToDto)];
    }

    public static SystemEndpointDto ConvertSystemEndpointEntitiesToDto(SystemEndpoint endpoint)
    {
        return new SystemEndpointDto
        {
            Name = endpoint.Name,
            BaseAddress = endpoint.BaseAddress.ToString()
        };
    }

    public static List<SystemEndpointDto> ConvertSystemEndpointEntitiesToDto(IReadOnlyList<SystemEndpoint> endpoints)
    {
        return [.. endpoints.Select(ConvertSystemEndpointEntitiesToDto)];
    }

    public static SystemCapabilityDto ConvertSystemCapabilityEntitiesToDto(SystemCapability capability)
    {
        return new SystemCapabilityDto
        {
            Name = capability.Name,
            Description = capability.Description
        };
    }

    public static List<SystemCapabilityDto> ConvertSystemCapabilityEntitiesToDto(IReadOnlyList<SystemCapability> capabilities)
    {
        return [.. capabilities.Select(ConvertSystemCapabilityEntitiesToDto)];
    }

    public async Task<RegisteredSystem?> GetSystemByIdAsync(GetSystemByIdQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        return await _systemRepository.GetByIdAsync(systemId, cancellationToken);
    }

    public async Task<RegisteredSystem?> GetSystemByNameAsync(GetSystemByNameQuery query, CancellationToken cancellationToken = default)
    {
        var name = SystemName.Create(query.Name);
        return await _systemRepository.GetByNameAsync(name, cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetAllSystemsAsync(CancellationToken cancellationToken = default)
    {
        return await _systemRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetSystemsByRegistrationStatusAsync(
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

        return await _systemRepository.GetByRegistrationStatusAsync(status, cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetSystemsByAvailabilityStatusAsync(
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

        return await _systemRepository.GetByAvailabilityStatusAsync(status, cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetSystemsByCapabilityAsync(GetSystemsByCapabilityQuery query, CancellationToken cancellationToken = default)
    {
        return await _systemRepository.GetByCapabilityNameAsync(query.CapabilityName.Trim(), cancellationToken);
    }

    public async Task<IReadOnlyList<SystemEndpoint>> GetSystemEndpointsAsync(GetSystemEndpointsQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        return await _systemRepository.GetAllEndpointsAsync(systemId, cancellationToken);
    }

    public async Task<IReadOnlyList<SystemCapability>> GetSystemCapabilitiesAsync(GetSystemCapabilitiesQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        return await _systemRepository.GetAllCapabilitiesAsync(systemId, cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetSystemsByDeletionStatusAsync(GetSystemsByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        return await _systemRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetDeletedSystemsAsync(CancellationToken cancellationToken = default)
    {
        return await _systemRepository.GetByDeletionStatusAsync(true, cancellationToken);
    }

    public static bool IsUnregistered(SystemDto system)
    {
        return Enum.TryParse<SystemRegistrationStatusFilter>(system.RegistrationStatus, out var status) && status == SystemRegistrationStatusFilter.Unregistered;
    }
}
