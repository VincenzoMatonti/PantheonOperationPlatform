using Athena.Api.WorkloadManagement.Responses;
using Athena.Application.WorkloadManagement.Contracts;

namespace Athena.Api.WorkloadManagement.Mappings;

public static class WorkloadManagementResponseMapper
{
    public static WorkloadLifecycleResponse ToResponse(WorkloadLifecycleDto dto) =>
        new(dto.WorkloadId, dto.DesiredState, dto.OperationalStatus, dto.CreatedAtUtc, dto.UpdatedAtUtc, dto.IsDeleted);

    public static IReadOnlyList<WorkloadLifecycleResponse> ToResponses(IEnumerable<WorkloadLifecycleDto> dtos) =>
        dtos.Select(ToResponse).ToArray();

    public static WorkloadOperationResponse ToResponse(WorkloadOperationDto dto) =>
        new(dto.Id, dto.WorkloadId, dto.Type, dto.Status, dto.CreatedAtUtc, dto.StartedAtUtc, dto.CompletedAtUtc, dto.ErrorMessage, dto.IsDeleted);

    public static IReadOnlyList<WorkloadOperationResponse> ToOperationResponses(IEnumerable<WorkloadOperationDto> dtos) =>
        dtos.Select(ToResponse).ToArray();
}
