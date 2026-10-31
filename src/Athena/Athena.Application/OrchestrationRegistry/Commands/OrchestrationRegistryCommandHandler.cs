using Athena.Application.OrchestrationRegistry.DTOs;
using Athena.Domain.Orchestration.Registry.Entities;
using Athena.Domain.Orchestration.Registry.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Application.OrchestrationRegistry.Commands;

public class OrchestrationRegistryCommandHandler(IOrchestrationRepository orchestrationRepository)
{
    private readonly IOrchestrationRepository _orchestrationRepository = orchestrationRepository;

    public async Task<OrchestrationDto> CreateAsync(CreateOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        OrchestrationId id = OrchestrationId.Create(command.OrchestrationId);
        OrchestrationName name = OrchestrationName.Create(command.Name);
        await EnsureNameIsUniqueAsync(name, null, cancellationToken);
        Orchestration orchestration = Orchestration.Create(id, name);
        await _orchestrationRepository.AddAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> RegisterAsync(RegisterOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.Register();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> UnregisterAsync(UnregisterOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.Unregister();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> EnableAsync(EnableOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.Enable();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> DisableAsync(DisableOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.Disable();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> RenameAsync(RenameOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        OrchestrationName newName = OrchestrationName.Create(command.Name);
        await EnsureNameIsUniqueAsync(newName, orchestration.Id.Value, cancellationToken);
        orchestration.Rename(newName);
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> MarkAvailableAsync(MarkOrchestrationAvailableCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.MarkAvailable();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> MarkUnavailableAsync(MarkOrchestrationUnavailableCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.MarkUnavailable();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> MarkAvailabilityUnknownAsync(
        MarkOrchestrationAvailabilityUnknownCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.MarkAvailabilityUnknown();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> DeleteAsync(DeleteOrchestrationCommand command, CancellationToken cancellationToken = default)
    {
        Orchestration? orchestration = await GetRequiredAsync(command.OrchestrationId, cancellationToken);
        if (orchestration is null) return null;
        orchestration.Delete();
        await _orchestrationRepository.UpdateAsync(orchestration, cancellationToken);
        return ToDto(orchestration);
    }

    private async Task<Orchestration?> GetRequiredAsync(Guid orchestrationId, CancellationToken cancellationToken)
    {
        OrchestrationId id = OrchestrationId.Create(orchestrationId);
        Orchestration? orchestration = await _orchestrationRepository.GetByIdAsync(id, cancellationToken);
        return orchestration;
    }

    private async Task EnsureNameIsUniqueAsync(OrchestrationName name, Guid? currentOrchestrationId, CancellationToken cancellationToken)
    {
        List<Orchestration> orchestrations = await _orchestrationRepository.GetAllAsync(cancellationToken);
        List<OrchestrationName> existingNames = [];
        foreach (Orchestration orchestration in orchestrations)
        {
            if (orchestration.IsDeleted) continue;
            if (currentOrchestrationId.HasValue && orchestration.Id.Value == currentOrchestrationId.Value) continue;
            existingNames.Add(orchestration.Name);
        }

        Orchestration.EnsureNameIsUnique(name, existingNames);
    }

    private static OrchestrationDto ToDto(Orchestration orchestration)
    {
        OrchestrationDto dto = new()
        {
            OrchestrationId = orchestration.Id.Value,
            Name = orchestration.Name.Value,
            RegistrationStatus = orchestration.RegistrationStatus.ToString(),
            AvailabilityStatus = orchestration.AvailabilityStatus.ToString(),
            CreatedAtUtc = orchestration.CreatedAtUtc,
            UpdatedAtUtc = orchestration.UpdatedAtUtc,
            IsDeleted = orchestration.IsDeleted
        };

        return dto;
    }
}
