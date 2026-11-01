using Athena.Api.EcosystemManagement.Responses;
using Athena.Application.EcosystemManagement.Contracts;

namespace Athena.Api.EcosystemManagement.Mappings;

public static class EcosystemManagementResponseMapper
{
    public static EcosystemLifecycleResponse ToResponse(EcosystemLifecycleDto dto) =>
        new(dto.EcosystemId, dto.DesiredState, dto.OperationalStatus, dto.CreatedAtUtc, dto.UpdatedAtUtc, dto.IsDeleted);

    public static IReadOnlyList<EcosystemLifecycleResponse> ToResponses(IEnumerable<EcosystemLifecycleDto> dtos) =>
        dtos.Select(ToResponse).ToArray();

    public static EcosystemOperationResponse ToResponse(EcosystemOperationDto dto) =>
        new(dto.Id, dto.EcosystemId, dto.Type, dto.Status, dto.CreatedAtUtc, dto.StartedAtUtc, dto.CompletedAtUtc, dto.ErrorMessage, dto.IsDeleted);

    public static IReadOnlyList<EcosystemOperationResponse> ToOperationResponses(IEnumerable<EcosystemOperationDto> dtos) =>
        dtos.Select(ToResponse).ToArray();
}
