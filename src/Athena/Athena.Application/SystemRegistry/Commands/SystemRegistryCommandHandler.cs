
using Athena.Application.SystemRegistry.Dtos;
using Athena.Domain.SystemRegistry.Repositories;
using Athena.Domain.SystemRegistry.ValueObjects;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;

namespace Athena.Application.SystemRegistry.Commands;

public class SystemRegistryCommandHandler(ISystemRepository systemRepository)
{
    private readonly ISystemRepository _systemRepository = systemRepository;

    public async Task<RegisteredSystem> CreateSystemAsync(RegisterSystemCommand command, CancellationToken cancellationToken = default)
    {
        var name = SystemName.Create(command.Name);
        var system = RegisteredSystem.Create(SystemId.New(), name);
        await _systemRepository.AddAsync(system, cancellationToken);
        return system;
    }

    public async Task RegisterExistingSystemAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.Register();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task UnregisterSystemAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.Unregister();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task RenameSystemAsync(RegisteredSystem system, string newName, CancellationToken cancellationToken = default)
    {
        var name = SystemName.Create(newName);
        system.Rename(name);
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task EnableSystemAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.Enable();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task DisableSystemAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.Disable();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task MarkSystemAvailableAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.MarkAvailable();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task MarkSystemUnavailableAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.MarkUnavailable();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task MarkSystemAvailabilityUnknownAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.MarkAvailabilityUnknown();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task AddSystemEndpointAsync(RegisteredSystem system, string name, string baseAddress, CancellationToken cancellationToken = default)
    {
        var endpoint = SystemEndpoint.Create(name, baseAddress);
        system.AddEndpoint(endpoint);
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task RemoveSystemEndpointAsync(RegisteredSystem system, string endpointName, CancellationToken cancellationToken = default)
    {
        system.RemoveEndpoint(endpointName);
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task AddSystemCapabilityAsync(RegisteredSystem system, string name, string description, CancellationToken cancellationToken = default)
    {
        var capability = SystemCapability.Create(name, description);
        system.AddCapability(capability);
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task RemoveSystemCapabilityAsync(RegisteredSystem system, string capabilityName, CancellationToken cancellationToken = default)
    {
        system.RemoveCapability(capabilityName);
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

    public async Task DeleteSystemAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        system.Delete();
        await _systemRepository.UpdateAsync(system, cancellationToken);
    }

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
}

