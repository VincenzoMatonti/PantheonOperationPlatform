using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.DTOs;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationSystemInstanceResponseMapper
{
    public static OrchestrationSystemInstanceCommandResponse ToCommandResponse(OrchestrationSystemInstanceDto dto)
    {
        return new OrchestrationSystemInstanceCommandResponse
        {
            Id = dto.Id,
            OrchestrationId = dto.OrchestrationId,
            SystemInstanceId = dto.SystemInstanceId,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static OrchestrationSystemInstanceQueryResponse ToQueryResponse(OrchestrationSystemInstanceDto dto)
    {
        return new OrchestrationSystemInstanceQueryResponse
        {
            Id = dto.Id,
            OrchestrationId = dto.OrchestrationId,
            SystemInstanceId = dto.SystemInstanceId,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static IReadOnlyList<OrchestrationSystemInstanceQueryResponse> ToQueryResponses(IEnumerable<OrchestrationSystemInstanceDto> dtos)
    {
        return [.. dtos.Select(ToQueryResponse)];
    }
}
