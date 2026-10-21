using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using ApplicationDesiredState = Athena.Application.OrchestrationManagement.Enums.OrchestrationDesiredState;
using ApplicationOperationalStatus = Athena.Application.OrchestrationManagement.Enums.OrchestrationOperationalStatus;
using DomainDesiredState = Athena.Domain.OrchestrationManagement.Enums.OrchestrationDesiredState;
using DomainOperationalStatus = Athena.Domain.OrchestrationManagement.Enums.OrchestrationOperationalStatus;

namespace Athena.Application.OrchestrationManagement.Queries;

public class OrchestrationLifecycleQueryHandler(IOrchestrationLifecycleRepository orchestrationLifecycleRepository)
{
    private readonly IOrchestrationLifecycleRepository _orchestrationLifecycleRepository = orchestrationLifecycleRepository;

    public async Task<OrchestrationLifecycleDto?> GetOrchestrationLifecycleByIdAsync(
        GetOrchestrationLifecycleByIdQuery query, CancellationToken cancellationToken = default)
    {
        var lifecycle = await _orchestrationLifecycleRepository.GetByIdAsync(OrchestrationId.Create(query.OrchestrationId), cancellationToken);
        return lifecycle is null ? null : ConvertOrchestrationLifecycleEntitiesToDto(lifecycle);
    }

    public async Task<List<OrchestrationLifecycleDto>> GetAllOrchestrationLifecyclesAsync(CancellationToken cancellationToken = default)
    {
        var lifecycles = await _orchestrationLifecycleRepository.GetAllAsync(cancellationToken);
        return [.. lifecycles.Select(ConvertOrchestrationLifecycleEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleDto>> GetOrchestrationLifecyclesByDesiredStateAsync(
        GetOrchestrationLifecyclesByDesiredStateQuery query, CancellationToken cancellationToken = default)
    {
        var state = ConvertDesiredStateToDomain(query.State);
        var lifecycles = await _orchestrationLifecycleRepository.GetByDesiredStateAsync(state, cancellationToken);
        return [.. lifecycles.Select(ConvertOrchestrationLifecycleEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleDto>> GetOrchestrationLifecyclesByOperationalStatusAsync(
        GetOrchestrationLifecyclesByOperationalStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = ConvertOperationalStatusToDomain(query.Status);
        var lifecycles = await _orchestrationLifecycleRepository.GetByOperationalStatusAsync(status, cancellationToken);
        return [.. lifecycles.Select(ConvertOrchestrationLifecycleEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleDto>> GetDeletedOrchestrationLifecyclesAsync(CancellationToken cancellationToken = default)
    {
        var lifecycles = await _orchestrationLifecycleRepository.GetDeletedAsync(cancellationToken);
        return [.. lifecycles.Select(ConvertOrchestrationLifecycleEntitiesToDto)];
    }

    // APPLICATION -> DOMAIN
    public static DomainDesiredState ConvertDesiredStateToDomain(ApplicationDesiredState state) => state switch
    {
        ApplicationDesiredState.Unknown => DomainDesiredState.Unknown,
        ApplicationDesiredState.Running => DomainDesiredState.Running,
        ApplicationDesiredState.Stopped => DomainDesiredState.Stopped,
        _ => DomainDesiredState.Unknown
    };

    public static DomainOperationalStatus ConvertOperationalStatusToDomain(ApplicationOperationalStatus status) => status switch
    {
        ApplicationOperationalStatus.Unknown => DomainOperationalStatus.Unknown,
        ApplicationOperationalStatus.Starting => DomainOperationalStatus.Starting,
        ApplicationOperationalStatus.Running => DomainOperationalStatus.Running,
        ApplicationOperationalStatus.Stopping => DomainOperationalStatus.Stopping,
        ApplicationOperationalStatus.Stopped => DomainOperationalStatus.Stopped,
        ApplicationOperationalStatus.Failed => DomainOperationalStatus.Failed,
        _ => DomainOperationalStatus.Unknown
    };

    // DOMAIN -> APPLICATION
    public static ApplicationDesiredState ConvertDesiredStateToApplication(DomainDesiredState state) => state switch
    {
        DomainDesiredState.Unknown => ApplicationDesiredState.Unknown,
        DomainDesiredState.Running => ApplicationDesiredState.Running,
        DomainDesiredState.Stopped => ApplicationDesiredState.Stopped,
        _ => ApplicationDesiredState.Unknown
    };

    public static ApplicationOperationalStatus ConvertOperationalStatusToApplication(DomainOperationalStatus status) => status switch
    {
        DomainOperationalStatus.Unknown => ApplicationOperationalStatus.Unknown,
        DomainOperationalStatus.Starting => ApplicationOperationalStatus.Starting,
        DomainOperationalStatus.Running => ApplicationOperationalStatus.Running,
        DomainOperationalStatus.Stopping => ApplicationOperationalStatus.Stopping,
        DomainOperationalStatus.Stopped => ApplicationOperationalStatus.Stopped,
        DomainOperationalStatus.Failed => ApplicationOperationalStatus.Failed,
        _ => ApplicationOperationalStatus.Unknown
    };

    public static OrchestrationLifecycleDto ConvertOrchestrationLifecycleEntitiesToDto(OrchestrationLifecycle lifecycle) => new()
    {
        OrchestrationId = lifecycle.Id.Value,
        DesiredState = ConvertDesiredStateToApplication(lifecycle.DesiredState),
        OperationalStatus = ConvertOperationalStatusToApplication(lifecycle.OperationalStatus),
        CreatedAtUtc = lifecycle.CreatedAtUtc,
        UpdatedAtUtc = lifecycle.UpdatedAtUtc,
        IsDeleted = lifecycle.IsDeleted
    };
}
