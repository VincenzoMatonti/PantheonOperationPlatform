using Athena.Application.OrchestrationRegistry.DTOs;
using Athena.Api.OrchestrationRegistry.Responses;

namespace Athena.Api.OrchestrationRegistry.Mappings;

public static class OrchestrationRegistryResponseMapper
{
    public static OrchestrationResponse ToResponse(OrchestrationDto dto)
    {
        return new OrchestrationResponse
        {
            OrchestrationId = dto.OrchestrationId,
            Name = dto.Name,
            RegistrationStatus = dto.RegistrationStatus,
            AvailabilityStatus = dto.AvailabilityStatus,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static IReadOnlyList<OrchestrationResponse> ToResponses(IEnumerable<OrchestrationDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }
}