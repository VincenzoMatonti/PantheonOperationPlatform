using Athena.Api.BlockManagement.Responses;
using Athena.Application.BlockManagement.Contracts;

namespace Athena.Api.BlockManagement.Mappings;

public static class BlockManagementResponseMapper
{
    public static BlockLifecycleResponse ToResponse(BlockLifecycleDto dto) =>
        new(dto.BlockId, dto.DesiredState, dto.OperationalStatus, dto.CreatedAtUtc, dto.UpdatedAtUtc, dto.IsDeleted);

    public static IReadOnlyList<BlockLifecycleResponse> ToResponses(IEnumerable<BlockLifecycleDto> dtos) =>
        dtos.Select(ToResponse).ToArray();

    public static BlockOperationResponse ToResponse(BlockOperationDto dto) =>
        new(dto.Id, dto.BlockId, dto.Type, dto.Status, dto.CreatedAtUtc, dto.StartedAtUtc, dto.CompletedAtUtc, dto.ErrorMessage, dto.IsDeleted);

    public static IReadOnlyList<BlockOperationResponse> ToOperationResponses(IEnumerable<BlockOperationDto> dtos) =>
        dtos.Select(ToResponse).ToArray();
}
