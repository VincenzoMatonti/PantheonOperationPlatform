using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Application.SystemRegistry.Dtos;

public static class SystemRegistryDtoMapper
{
    public static SystemDto ToDto(RegisteredSystem system) =>
        new()
        {
            Id = system.Id.Value,
            Name = system.Name.Value,
            RegistrationStatus = system.RegistrationStatus.ToString(),
            AvailabilityStatus = system.AvailabilityStatus.ToString(),
            Endpoints = system.Endpoints.Select(ToDto).ToArray(),
            Capabilities = system.Capabilities.Select(ToDto).ToArray(),
            CreatedAtUtc = system.CreatedAtUtc,
            UpdatedAtUtc = system.UpdatedAtUtc,
            IsDeleted = system.IsDeleted
        };

    public static List<SystemDto> ToDtos(IEnumerable<RegisteredSystem> systems) =>
        systems.Select(ToDto).ToList();

    public static SystemEndpointDto ToDto(SystemEndpoint endpoint) =>
        new() { Name = endpoint.Name, BaseAddress = endpoint.BaseAddress.ToString() };

    public static List<SystemEndpointDto> ToEndpointDtos(IEnumerable<SystemEndpoint> endpoints) =>
        endpoints.Select(ToDto).ToList();

    public static SystemCapabilityDto ToDto(SystemCapability capability) =>
        new() { Name = capability.Name, Description = capability.Description };

    public static List<SystemCapabilityDto> ToCapabilityDtos(IEnumerable<SystemCapability> capabilities) =>
        capabilities.Select(ToDto).ToList();
}
