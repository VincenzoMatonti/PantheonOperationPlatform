
using Athena.Application.SystemRegistry.Dtos;
using Athena.Api.SystemRegistry.Responses;

namespace Athena.Api.SystemRegistry.Mappings;

public static class SystemRegistryResponseMapper
{
    public static SystemResponse ToResponse(SystemDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        RegistrationStatus = dto.RegistrationStatus,
        AvailabilityStatus = dto.AvailabilityStatus,
        Endpoints = dto.Endpoints.Select(ToResponse).ToArray(),
        Capabilities = dto.Capabilities.Select(ToResponse).ToArray(),
        CreatedAtUtc = dto.CreatedAtUtc,
        UpdatedAtUtc = dto.UpdatedAtUtc,
        IsDeleted = dto.IsDeleted
    };

    public static SystemEndpointResponse ToResponse(SystemEndpointDto dto) => new()
    {
        Name = dto.Name,
        BaseAddress = dto.BaseAddress
    };

    public static SystemCapabilityResponse ToResponse(SystemCapabilityDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description
    };

    public static IReadOnlyList<SystemResponse> ToResponse(
        IEnumerable<SystemDto> systems) => [.. systems.Select(ToResponse)];

    public static IReadOnlyList<SystemEndpointResponse> ToEndpointResponses(
        IEnumerable<SystemEndpointDto> endpoints) => [.. endpoints.Select(ToResponse)];

    public static IReadOnlyList<SystemCapabilityResponse> ToCapabilityResponses(
        IEnumerable<SystemCapabilityDto> capabilities) => [.. capabilities.Select(ToResponse)];
}
