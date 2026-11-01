using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.Common.Services;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationAggregateStatusResponseMapper
{
    public static OrchestrationAggregateStatusResponse ToResponse(OrchestrationAggregateStatusDto dto) =>
        new(dto.OrchestrationId, dto.AggregateStatus.ToString(), dto.DesiredState, dto.ObservedStatus, dto.LatestObservedAtUtc);
}
