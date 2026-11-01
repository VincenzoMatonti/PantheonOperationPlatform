using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.Commands;

public class OrchestrationSystemInstanceCommandHandler(IOrchestrationSystemInstanceRepository orchestrationSystemRepository)
{
    private readonly IOrchestrationSystemInstanceRepository _orchestrationSystemRepository = orchestrationSystemRepository;

    public async Task<OrchestrationSystemInstanceDto> CreateOrchestrationSystemInstanceAsync(CreateOrchestrationSystemInstanceCommand command, CancellationToken cancellationToken = default)
    {
        var orchestrationId = OrchestrationId.Create(command.OrchestrationId);
        var systemInstanceId = SystemInstanceId.Create(command.SystemInstanceId);
        var associationExists = await _orchestrationSystemRepository.AssociationExistsAsync(orchestrationId, systemInstanceId, includeDeleted: true, cancellationToken);
        if (associationExists) throw new OrchestrationSystemInstanceAlreadyExistsException(command.OrchestrationId, command.SystemInstanceId);
        var association = OrchestrationSystemInstance.Create(orchestrationId, systemInstanceId);
        await _orchestrationSystemRepository.AddAsync(association, cancellationToken);
        return ConvertOrchestrationSystemInstanceEntitiesToDto(association);
    }

    public Task<OrchestrationSystemInstanceDto?> DeleteOrchestrationSystemInstanceAsync(DeleteOrchestrationSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationSystemInstanceAsync(command.Id, association => association.Delete(), includeDeleted: false, cancellationToken);

    public Task<OrchestrationSystemInstanceDto?> RestoreOrchestrationSystemInstanceAsync(RestoreOrchestrationSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationSystemInstanceAsync(command.Id, association => association.Restore(), includeDeleted: true, cancellationToken);

    private async Task<OrchestrationSystemInstanceDto?> UpdateOrchestrationSystemInstanceAsync(
        Guid id, Action<OrchestrationSystemInstance> update, bool includeDeleted, CancellationToken cancellationToken)
    {
        var associationId = OrchestrationSystemInstanceId.Create(id);
        var association = await _orchestrationSystemRepository.GetByIdAsync(associationId, includeDeleted, cancellationToken);
        if (association is null) return null;
        update(association);
        await _orchestrationSystemRepository.UpdateAsync(association, cancellationToken);
        return ConvertOrchestrationSystemInstanceEntitiesToDto(association);
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
