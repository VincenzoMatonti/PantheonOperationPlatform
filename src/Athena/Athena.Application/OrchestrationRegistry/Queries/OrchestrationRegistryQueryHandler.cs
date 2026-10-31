using Athena.Application.OrchestrationRegistry.DTOs;
using Athena.Domain.Orchestration.Registry.Entities;
using Athena.Domain.Orchestration.Registry.Enums;
using Athena.Domain.Orchestration.Registry.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Application.OrchestrationRegistry.Queries;

public sealed class OrchestrationRegistryQueryHandler(IOrchestrationRepository orchestrationRepository)
{
    private readonly IOrchestrationRepository _orchestrationRepository = orchestrationRepository;

    public async Task<OrchestrationDto?> GetByIdAsync(GetOrchestrationByIdQuery query, CancellationToken cancellationToken = default)
    {
        OrchestrationId id = OrchestrationId.Create(query.OrchestrationId);
        Orchestration? orchestration = await _orchestrationRepository.GetByIdAsync(id, cancellationToken);
        if (orchestration is null) return null;
        return ToDto(orchestration);
    }

    public async Task<OrchestrationDto?> GetByNameAsync(GetOrchestrationByNameQuery query, CancellationToken cancellationToken = default)
    {
        OrchestrationName name = OrchestrationName.Create(query.Name);
        Orchestration? orchestration = await _orchestrationRepository.GetByNameAsync(name, cancellationToken);
        if (orchestration is null) return null;
        return ToDto(orchestration);
    }

    public async Task<List<OrchestrationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<Orchestration> orchestrations = await _orchestrationRepository.GetAllAsync(cancellationToken);
        List<OrchestrationDto> dtos = [];
        foreach (Orchestration orchestration in orchestrations)
        {
            dtos.Add(ToDto(orchestration));
        }

        return dtos;
    }

    public async Task<List<OrchestrationDto>> GetByRegistrationStatusAsync(
        GetOrchestrationsByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        OrchestrationRegistrationStatus status = (OrchestrationRegistrationStatus)(int)query.Status;
        List<Orchestration> orchestrations = await _orchestrationRepository.GetByRegistrationStatusAsync(status, cancellationToken);
        List<OrchestrationDto> dtos = [];
        foreach (Orchestration orchestration in orchestrations)
        {
            dtos.Add(ToDto(orchestration));
        }

        return dtos;
    }

    public async Task<List<OrchestrationDto>> GetByAvailabilityStatusAsync(
        GetOrchestrationsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default)
    {
        OrchestrationAvailabilityStatus status = (OrchestrationAvailabilityStatus)(int)query.Status;
        List<Orchestration> orchestrations = await _orchestrationRepository.GetByAvailabilityStatusAsync(status, cancellationToken);
        List<OrchestrationDto> dtos = [];
        foreach (Orchestration orchestration in orchestrations)
        {
            dtos.Add(ToDto(orchestration));
        }

        return dtos;
    }

    public async Task<List<OrchestrationDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        List<Orchestration> orchestrations = await _orchestrationRepository.GetDeletedAsync(cancellationToken);
        List<OrchestrationDto> dtos = [];
        foreach (Orchestration orchestration in orchestrations)
        {
            dtos.Add(ToDto(orchestration));
        }

        return dtos;
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
