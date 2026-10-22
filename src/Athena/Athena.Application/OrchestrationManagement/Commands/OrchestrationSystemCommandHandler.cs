using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationManagement.ValueObjects;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.Commands;

public class OrchestrationSystemCommandHandler(IOrchestrationSystemRepository orchestrationSystemRepository)
{
    private readonly IOrchestrationSystemRepository _orchestrationSystemRepository = orchestrationSystemRepository;

    public async Task<OrchestrationSystemDto> CreateOrchestrationSystemAsync(CreateOrchestrationSystemCommand command, CancellationToken cancellationToken = default)
    {
        var orchestrationId = OrchestrationId.Create(command.OrchestrationId);
        var systemId = SystemId.Create(command.SystemId);
        var associationExists = await _orchestrationSystemRepository.AssociationExistsAsync(orchestrationId, systemId, includeDeleted: true, cancellationToken);
        if (associationExists) throw new OrchestrationSystemAlreadyExistsException(command.OrchestrationId, command.SystemId);
        var association = OrchestrationSystem.Create(orchestrationId, systemId);
        await _orchestrationSystemRepository.AddAsync(association, cancellationToken);
        return ConvertOrchestrationSystemEntitiesToDto(association);
    }

    public Task<OrchestrationSystemDto?> DeleteOrchestrationSystemAsync(DeleteOrchestrationSystemCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationSystemAsync(command.Id, association => association.Delete(), includeDeleted: false, cancellationToken);

    public Task<OrchestrationSystemDto?> RestoreOrchestrationSystemAsync(RestoreOrchestrationSystemCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationSystemAsync(command.Id, association => association.Restore(), includeDeleted: true, cancellationToken);

    private async Task<OrchestrationSystemDto?> UpdateOrchestrationSystemAsync(
        Guid id, Action<OrchestrationSystem> update, bool includeDeleted, CancellationToken cancellationToken)
    {
        var associationId = OrchestrationSystemId.Create(id);
        var association = await _orchestrationSystemRepository.GetByIdAsync(associationId, includeDeleted, cancellationToken);
        if (association is null) return null;
        update(association);
        _ = _orchestrationSystemRepository.UpdateAsync(association, cancellationToken);
        return ConvertOrchestrationSystemEntitiesToDto(association);
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
