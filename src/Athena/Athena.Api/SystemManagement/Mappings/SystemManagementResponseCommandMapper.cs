using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.Dtos;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemManagementResponseCommandMapper
{
    public static SystemLifecycleCommandResponse ToResponse(SystemLifecycleDto dto) => new()
    {
        SystemId = dto.SystemId,
        DesiredState = dto.DesiredState,
        OperationalStatus = dto.OperationalStatus,
        CreatedAtUtc = dto.CreatedAtUtc,
        UpdatedAtUtc = dto.UpdatedAtUtc,
        IsDeleted = dto.IsDeleted
    };

    public static SystemLifecycleOperationCommandResponse ToResponse(SystemLifecycleOperationDto dto) => new()
    {
        Id = dto.Id,
        SystemId = dto.SystemId,
        Type = dto.Type,
        Status = dto.Status,
        CreatedAtUtc = dto.CreatedAtUtc,
        UpdatedAtUtc = dto.UpdatedAtUtc,
        StartedAtUtc = dto.StartedAtUtc,
        CompletedAtUtc = dto.CompletedAtUtc,
        ErrorMessage = dto.ErrorMessage,
        IsDeleted = dto.IsDeleted
    };
}