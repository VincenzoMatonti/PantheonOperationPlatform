using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.DTOs;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationSystemResponseMapper
{
    public static OrchestrationSystemCommandResponse ToCommandResponse(OrchestrationSystemDto dto)
    {
        return new OrchestrationSystemCommandResponse
        {
            Id = dto.Id,
            OrchestrationId = dto.OrchestrationId,
            SystemId = dto.SystemId,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static OrchestrationSystemQueryResponse ToQueryResponse(OrchestrationSystemDto dto)
    {
        return new OrchestrationSystemQueryResponse
        {
            Id = dto.Id,
            OrchestrationId = dto.OrchestrationId,
            SystemId = dto.SystemId,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static IReadOnlyList<OrchestrationSystemQueryResponse> ToQueryResponses(IEnumerable<OrchestrationSystemDto> dtos)
    {
        return [.. dtos.Select(ToQueryResponse)];
    }
}
