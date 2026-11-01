using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.Queries;

public class OrchestrationSystemInstanceQueryHandler(IOrchestrationSystemInstanceRepository orchestrationSystemRepository)
{
    private readonly IOrchestrationSystemInstanceRepository _orchestrationSystemRepository = orchestrationSystemRepository;

    public async Task<OrchestrationSystemInstanceDto?> GetOrchestrationSystemInstanceByIdAsync(GetOrchestrationSystemInstanceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var associationId = OrchestrationSystemInstanceId.Create(query.Id);
        var association = await _orchestrationSystemRepository.GetByIdAsync(associationId, includeDeleted: false, cancellationToken);
        return association is null ? null : ConvertOrchestrationSystemInstanceEntitiesToDto(association);
    }

    public async Task<List<OrchestrationSystemInstanceDto>> GetAllOrchestrationSystemInstancesAsync(GetOrchestrationSystemInstancesQuery query, CancellationToken cancellationToken = default)
    {
        var associations = await _orchestrationSystemRepository.GetAllAsync(query.IncludeDeleted, cancellationToken);
        return [.. associations.Select(ConvertOrchestrationSystemInstanceEntitiesToDto)];
    }

    public async Task<IReadOnlyList<OrchestrationSystemInstanceDto>> GetByOrchestrationIdAsync(
        GetOrchestrationSystemInstancesByOrchestrationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var orchestrationId = OrchestrationId.Create(query.OrchestrationId);
        var associations = await _orchestrationSystemRepository.GetByOrchestrationIdAsync(
            orchestrationId, includeDeleted: false, cancellationToken);
        return associations.Select(ConvertOrchestrationSystemInstanceEntitiesToDto).ToArray();
    }

    public static OrchestrationSystemInstanceDto ConvertOrchestrationSystemInstanceEntitiesToDto(OrchestrationSystemInstance association) => new()
    {
        Id = association.Id.Value,
        OrchestrationId = association.OrchestrationId.Value,
        SystemInstanceId = association.SystemInstanceId.Value,
        CreatedAtUtc = association.CreatedAtUtc,
        UpdatedAtUtc = association.UpdatedAtUtc,
        IsDeleted = association.IsDeleted
    };
}
