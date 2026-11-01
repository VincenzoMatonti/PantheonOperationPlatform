using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.Common.Queries;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.Extensions.Logging;
using DomainOperationStatus = Athena.Domain.Orchestration.Management.Enums.OrchestrationLifecycleOperationStatus;

namespace Athena.Application.Common.Services;

public sealed record OrchestrationAggregateStatusDto(
    Guid OrchestrationId,
    ResourceAggregateStatus AggregateStatus,
    string DesiredState,
    string ObservedStatus,
    DateTime? LatestObservedAtUtc);

public sealed class OrchestrationStatusSynchronizationService(
    IOrchestrationSystemInstanceRepository associationRepository,
    IOrchestrationLifecycleOperationRepository operationRepository,
    IOrchestrationResourceCoordinator resourceCoordinator,
    ILogger<OrchestrationStatusSynchronizationService> logger) : IOrchestrationStatusSynchronizer
{
    public async Task<OrchestrationAggregateStatusDto> GetAggregateStatusAsync(
        Guid orchestrationId,
        CancellationToken cancellationToken = default)
    {
        var associations = await associationRepository.GetByOrchestrationIdAsync(
            OrchestrationId.Create(orchestrationId),
            cancellationToken: cancellationToken);
        if (associations.Count == 0)
            return new OrchestrationAggregateStatusDto(orchestrationId, ResourceAggregateStatus.Unknown, "Unknown", "Unknown", null);

        var summary = await resourceCoordinator.GetLifecycleSummaryAsync(
            associations.Select(association => association.SystemInstanceId.Value),
            cancellationToken);
        return new OrchestrationAggregateStatusDto(
            orchestrationId,
            summary.AggregateStatus,
            summary.DesiredState,
            summary.OperationalStatus,
            summary.LatestObservedAtUtc);
    }

    public Task<OrchestrationAggregateStatusDto> GetAggregateStatusAsync(
        GetOrchestrationAggregateStatusQuery query,
        CancellationToken cancellationToken = default) =>
        GetAggregateStatusAsync(query.OrchestrationId, cancellationToken);

    public async Task SynchronizeForSystemInstancesAsync(
        IEnumerable<Guid> systemInstanceIds,
        CancellationToken cancellationToken = default,
        Guid? excludedOrchestrationId = null)
    {
        ArgumentNullException.ThrowIfNull(systemInstanceIds);
        var affectedOrchestrationIds = new HashSet<Guid>();
        foreach (var systemInstanceId in systemInstanceIds.Distinct())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var associations = await associationRepository.GetBySystemInstanceIdAsync(
                SystemInstanceId.Create(systemInstanceId),
                cancellationToken: cancellationToken);
            foreach (var association in associations)
            {
                if (association.OrchestrationId.Value != excludedOrchestrationId)
                    affectedOrchestrationIds.Add(association.OrchestrationId.Value);
            }
        }

        foreach (var orchestrationId in affectedOrchestrationIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var pending = await operationRepository.GetByOrchestrationIdAndStatusAsync(
                OrchestrationId.Create(orchestrationId),
                DomainOperationStatus.Pending,
                cancellationToken);
            var running = await operationRepository.GetByOrchestrationIdAndStatusAsync(
                OrchestrationId.Create(orchestrationId),
                DomainOperationStatus.Running,
                cancellationToken);
            if (pending.Count > 0 || running.Count > 0)
            {
                logger.LogDebug("Skipping aggregate synchronization for Orchestration {OrchestrationId} while an Operation is active.", orchestrationId);
                continue;
            }

            var aggregate = await GetAggregateStatusAsync(orchestrationId, cancellationToken);
            logger.LogInformation(
                "Orchestration {OrchestrationId} derived aggregate status is {AggregateStatus}; aggregate synchronization does not modify its Lifecycle.",
                orchestrationId,
                aggregate.AggregateStatus);
        }
    }
}
