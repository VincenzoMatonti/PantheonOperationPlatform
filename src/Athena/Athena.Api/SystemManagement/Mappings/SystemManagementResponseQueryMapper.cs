using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.Dtos;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemManagementResponseQueryMapper
{
    public static SystemLifecycleQueryResponse ToResponse(SystemLifecycleDto dto) => new()
    {
        SystemId = dto.SystemId,
        DesiredState = dto.DesiredState,
        OperationalStatus = dto.OperationalStatus,
        CreatedAtUtc = dto.CreatedAtUtc,
        UpdatedAtUtc = dto.UpdatedAtUtc,
        IsDeleted = dto.IsDeleted
    };

    public static IReadOnlyList<SystemLifecycleQueryResponse> ToLifecycleResponses(IEnumerable<SystemLifecycleDto> dtos) =>
    [
        .. dtos.Select(ToResponse)
    ];

    public static SystemLifecycleOperationQueryResponse ToResponse(SystemLifecycleOperationDto dto) => new()
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

    public static IReadOnlyList<SystemLifecycleOperationQueryResponse> ToOperationResponses(IEnumerable<SystemLifecycleOperationDto> dtos) =>
    [
        .. dtos.Select(ToResponse)
    ];
}