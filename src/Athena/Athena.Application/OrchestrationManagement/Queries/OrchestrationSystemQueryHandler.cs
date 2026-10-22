using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationManagement.ValueObjects;

namespace Athena.Application.OrchestrationManagement.Queries;

public class OrchestrationSystemQueryHandler(IOrchestrationSystemRepository orchestrationSystemRepository)
{
    private readonly IOrchestrationSystemRepository _orchestrationSystemRepository = orchestrationSystemRepository;

    public async Task<OrchestrationSystemDto?> GetOrchestrationSystemByIdAsync(GetOrchestrationSystemByIdQuery query, CancellationToken cancellationToken = default)
    {
        var associationId = OrchestrationSystemId.Create(query.Id);
        var association = await _orchestrationSystemRepository.GetByIdAsync(associationId, includeDeleted: false, cancellationToken);
        return association is null ? null : ConvertOrchestrationSystemEntitiesToDto(association);
    }

    public async Task<List<OrchestrationSystemDto>> GetAllOrchestrationSystemsAsync(GetOrchestrationSystemsQuery query, CancellationToken cancellationToken = default)
    {
        var associations = await _orchestrationSystemRepository.GetAllAsync(query.IncludeDeleted, cancellationToken);
        return [.. associations.Select(ConvertOrchestrationSystemEntitiesToDto)];
    }

    public static OrchestrationSystemDto ConvertOrchestrationSystemEntitiesToDto(OrchestrationSystem association) => new()
    {
        Id = association.Id.Value,
        OrchestrationId = association.OrchestrationId.Value,
        SystemId = association.SystemId.Value,
        CreatedAtUtc = association.CreatedAtUtc,
        UpdatedAtUtc = association.UpdatedAtUtc,
        IsDeleted = association.IsDeleted
    };
}
