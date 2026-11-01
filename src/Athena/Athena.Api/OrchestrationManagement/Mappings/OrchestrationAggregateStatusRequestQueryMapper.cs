using Athena.Application.Common.Queries;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationAggregateStatusRequestQueryMapper
{
    public static GetOrchestrationAggregateStatusQuery ToQuery(Guid orchestrationId) => new(orchestrationId);
}
