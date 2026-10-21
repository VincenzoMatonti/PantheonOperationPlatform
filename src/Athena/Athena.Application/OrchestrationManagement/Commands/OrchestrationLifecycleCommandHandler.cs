using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using ApplicationDesiredState = Athena.Application.OrchestrationManagement.Enums.OrchestrationDesiredState;
using ApplicationOperationalStatus = Athena.Application.OrchestrationManagement.Enums.OrchestrationOperationalStatus;
using DomainDesiredState = Athena.Domain.OrchestrationManagement.Enums.OrchestrationDesiredState;
using DomainOperationalStatus = Athena.Domain.OrchestrationManagement.Enums.OrchestrationOperationalStatus;

namespace Athena.Application.OrchestrationManagement.Commands;

public class OrchestrationLifecycleCommandHandler(IOrchestrationLifecycleRepository orchestrationLifecycleRepository)
{
    private readonly IOrchestrationLifecycleRepository _orchestrationLifecycleRepository = orchestrationLifecycleRepository;

    public async Task<OrchestrationLifecycleDto> CreateOrchestrationLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default)
    {
        var id = OrchestrationId.Create(orchestrationId);
        var lifecycle = OrchestrationLifecycle.Create(id);
        await _orchestrationLifecycleRepository.AddAsync(lifecycle, cancellationToken);
        return ConvertOrchestrationLifecycleEntitiesToDto(lifecycle);
    }

    public Task<OrchestrationLifecycleDto?> SetOrchestrationDesiredStateAsync(
        SetOrchestrationDesiredStateCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.SetDesiredState(
            ConvertDesiredStateToDomain(command.DesiredState)), cancellationToken);

    public Task<OrchestrationLifecycleDto?> MarkOrchestrationStartingAsync(
        MarkOrchestrationStartingCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.MarkStarting(), cancellationToken);

    public Task<OrchestrationLifecycleDto?> MarkOrchestrationRunningAsync(
        MarkOrchestrationRunningCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.MarkRunning(), cancellationToken);

    public Task<OrchestrationLifecycleDto?> MarkOrchestrationStoppingAsync(
        MarkOrchestrationStoppingCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.MarkStopping(), cancellationToken);

    public Task<OrchestrationLifecycleDto?> MarkOrchestrationStoppedAsync(
        MarkOrchestrationStoppedCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.MarkStopped(), cancellationToken);

    public Task<OrchestrationLifecycleDto?> MarkOrchestrationFailedAsync(
        MarkOrchestrationFailedCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.MarkFailed(), cancellationToken);

    public Task<OrchestrationLifecycleDto?> MarkOrchestrationOperationalStatusUnknownAsync(
        MarkOrchestrationOperationalStatusUnknownCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.MarkOperationalStatusUnknown(), cancellationToken);

    public Task<OrchestrationLifecycleDto?> DeleteOrchestrationLifecycleAsync(
        DeleteOrchestrationLifecycleCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleAsync(command.OrchestrationId, lifecycle => lifecycle.Delete(), cancellationToken);

    private async Task<OrchestrationLifecycleDto?> UpdateOrchestrationLifecycleAsync(
        Guid orchestrationId, Action<OrchestrationLifecycle> update, CancellationToken cancellationToken)
    {
        var id = OrchestrationId.Create(orchestrationId);
        var lifecycle = await _orchestrationLifecycleRepository.GetByIdAsync(id, cancellationToken);
        if (lifecycle is null) return null;
        update(lifecycle);
        await _orchestrationLifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
        return ConvertOrchestrationLifecycleEntitiesToDto(lifecycle);
    }

    // APPLICATION -> DOMAIN
    // APPLICATION -> DOMAIN
public static DomainDesiredState ConvertDesiredStateToDomain(
    ApplicationDesiredState state) => state switch
{
    ApplicationDesiredState.Unknown => DomainDesiredState.Unknown,
    ApplicationDesiredState.Running => DomainDesiredState.Running,
    ApplicationDesiredState.Stopped => DomainDesiredState.Stopped,
    _ => DomainDesiredState.Unknown
};

// DOMAIN -> APPLICATION
public static ApplicationDesiredState ConvertDesiredStateToApplication(
    DomainDesiredState state) => state switch
{
    DomainDesiredState.Unknown => ApplicationDesiredState.Unknown,
    DomainDesiredState.Running => ApplicationDesiredState.Running,
    DomainDesiredState.Stopped => ApplicationDesiredState.Stopped,
    _ => ApplicationDesiredState.Unknown
};

public static ApplicationOperationalStatus ConvertOperationalStatusToApplication(
    DomainOperationalStatus status) => status switch
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
