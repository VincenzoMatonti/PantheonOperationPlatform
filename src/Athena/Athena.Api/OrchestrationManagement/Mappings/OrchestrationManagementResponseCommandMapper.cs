using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.DTOs;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationManagementResponseCommandMapper
{
    public static OrchestrationLifecycleCommandResponse ToResponse(OrchestrationLifecycleDto dto)
    {
        return new OrchestrationLifecycleCommandResponse
        {
            OrchestrationId = dto.OrchestrationId,
            DesiredState = dto.DesiredState.ToString(),
            OperationalStatus = dto.OperationalStatus.ToString(),
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc,
            IsDeleted = dto.IsDeleted
        };
    }

    public static OrchestrationLifecycleOperationCommandResponse ToResponse(OrchestrationLifecycleOperationDto dto)
    {
        return new OrchestrationLifecycleOperationCommandResponse
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
}