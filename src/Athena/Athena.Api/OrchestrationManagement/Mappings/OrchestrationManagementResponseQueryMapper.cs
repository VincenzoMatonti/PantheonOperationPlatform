using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.DTOs;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationManagementResponseQueryMapper
{
    public static OrchestrationLifecycleQueryResponse ToResponse(OrchestrationLifecycleDto dto)
    {
        return new OrchestrationLifecycleQueryResponse
        {
            OrchestrationId = dto.OrchestrationId,
            DesiredState = dto.DesiredState.ToString(),
            OperationalStatus = dto.OperationalStatus.ToString(),
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static IReadOnlyList<OrchestrationLifecycleQueryResponse> ToLifecycleResponses(IEnumerable<OrchestrationLifecycleDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }

    public static OrchestrationLifecycleOperationQueryResponse ToResponse(OrchestrationLifecycleOperationDto dto)
    {
        return new OrchestrationLifecycleOperationQueryResponse
        {
            Id = dto.Id,
            OrchestrationId = dto.OrchestrationId,
            Type = dto.Type.ToString(),
            Status = dto.Status.ToString(),
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            StartedAtUtc = dto.StartedAtUtc,
            CompletedAtUtc = dto.CompletedAtUtc,
            ErrorMessage = dto.ErrorMessage,
            IsDeleted = dto.IsDeleted
        };
    }

    public static IReadOnlyList<OrchestrationLifecycleOperationQueryResponse> ToOperationResponses(IEnumerable<OrchestrationLifecycleOperationDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }
}