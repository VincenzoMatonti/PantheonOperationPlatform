using Athena.Application.Common.Services;
using Athena.Application.BackgroundTasks.Synchronization.Interfaces;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.UseCases;
using Athena.Application.SystemRegistry.Commands;
using Athena.Application.SystemRegistry.UseCases;
using Athena.Application.Common.Dtos;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.Enums;
using Microsoft.Extensions.Logging;

namespace Athena.Application.BackgroundTasks.Synchronization;

public sealed class SystemSynchronizationService(
    RuntimeApplicationService runtimeApplicationService,
    SystemRegistryUseCaseHandler systemRegistryUseCaseHandler,
    SystemManagementUseCaseHandler systemManagementUseCaseHandler,
    OrchestrationStatusSynchronizationService orchestrationStatusSynchronizationService,
    IEndpointAvailabilityChecker endpointAvailabilityChecker,
    EndpointCheckSchedule endpointCheckSchedule,
    ILogger<SystemSynchronizationService> logger)
{
    private static readonly TimeSpan EndpointCheckInterval = TimeSpan.FromMinutes(5);

    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var nowUtc = DateTime.UtcNow;
        var diagnostics = await runtimeApplicationService.DiscoverWithDiagnosticsAsync(cancellationToken);
        var changedOperationalSystemIds = new List<Guid>();
        await SynchronizeOperationalStatusesAsync(diagnostics, changedOperationalSystemIds, cancellationToken);
        await SynchronizeAvailabilityStatusesAsync(nowUtc, cancellationToken);
        if (changedOperationalSystemIds.Count > 0) await orchestrationStatusSynchronizationService.SynchronizeForSystemsAsync(changedOperationalSystemIds, cancellationToken);
        logger.LogInformation("System synchronization completed. Discovered instances: {InstanceCount}. Operational changes: {ChangedCount}.", diagnostics.Count, changedOperationalSystemIds.Count);
    }

    private async Task SynchronizeOperationalStatusesAsync(IReadOnlyList<RuntimeInstanceDiagnosticsDto> diagnostics, List<Guid> changedSystemIds, CancellationToken cancellationToken)
    {
        var lifecycles = await systemManagementUseCaseHandler.GetAllSystemLifecyclesAsync(cancellationToken);
        var systems = diagnostics.GroupBy(item => item.Instance.SystemId);
        foreach (var systemGroup in systems)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var systemId = systemGroup.Key;
            var instances = systemGroup.ToList();
            var lifecycle = lifecycles.FirstOrDefault(item => item.SystemId == systemId);
            if (lifecycle is null || lifecycle.IsDeleted) continue;
            var operationalStatus = DetermineOperationalStatus(instances);
            LogHealthChecks(systemId, instances);
            if (lifecycle.OperationalStatus == operationalStatus) continue;
            await UpdateOperationalStatusAsync(systemId, operationalStatus, cancellationToken);
            changedSystemIds.Add(systemId);
            logger.LogInformation("System {SystemId} operational status changed from {PreviousStatus} to {CurrentStatus}.", systemId, lifecycle.OperationalStatus, operationalStatus);
        }
    }

    private async Task SynchronizeAvailabilityStatusesAsync(DateTime nowUtc, CancellationToken cancellationToken)
    {
        var systems = await systemRegistryUseCaseHandler.GetAllSystemsAsync(cancellationToken);
        foreach (var system in systems)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (system.IsDeleted || !string.Equals(system.RegistrationStatus, "Registered", StringComparison.OrdinalIgnoreCase)) continue;
            foreach (var endpoint in system.Endpoints)
            {
                if (!endpointCheckSchedule.ShouldCheck(system.Id, endpoint.Name, endpoint.BaseAddress, nowUtc, EndpointCheckInterval)) continue;
                bool isAvailable;

                try
                {
                    isAvailable = await endpointAvailabilityChecker.CheckAvailabilityAsync(endpoint.BaseAddress, cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    isAvailable = false;
                    logger.LogWarning(exception, "Availability check failed for endpoint {EndpointName} of system {SystemId}.", endpoint.Name, system.Id);
                }

                endpointCheckSchedule.RecordResult(system.Id, endpoint.Name, endpoint.BaseAddress, isAvailable, nowUtc);
            }
            var availabilityStatus = endpointCheckSchedule.DetermineAvailability(system.Id, system.Endpoints);
            if (string.Equals(system.AvailabilityStatus, availabilityStatus, StringComparison.OrdinalIgnoreCase)) continue;
            await UpdateAvailabilityStatusAsync(system.Id, availabilityStatus, cancellationToken);
            logger.LogInformation("System {SystemId} availability status changed from {PreviousStatus} to {CurrentStatus}.", system.Id, system.AvailabilityStatus, availabilityStatus);
        }
    }

    private async Task UpdateOperationalStatusAsync(Guid systemId, string operationalStatus, CancellationToken cancellationToken)
    {
        switch (operationalStatus)
        {
            case nameof(SystemOperationalStatus.Running):
                await systemManagementUseCaseHandler.MarkSystemRunningAsync(new MarkSystemRunningCommand { SystemId = systemId }, cancellationToken);
                break;

            case nameof(SystemOperationalStatus.Stopped):
                await systemManagementUseCaseHandler.MarkSystemStoppedAsync(new MarkSystemStoppedCommand { SystemId = systemId }, cancellationToken);
                break;

            case nameof(SystemOperationalStatus.Failed):
                await systemManagementUseCaseHandler.MarkSystemFailedAsync(new MarkSystemFailedCommand { SystemId = systemId }, cancellationToken);
                break;

            default:
                await systemManagementUseCaseHandler.MarkSystemUnknownAsync(new MarkSystemUnknownCommand { SystemId = systemId }, cancellationToken);
                break;
        }
    }

    private async Task UpdateAvailabilityStatusAsync(Guid systemId, string availabilityStatus, CancellationToken cancellationToken)
    {
        switch (availabilityStatus)
        {
            case nameof(SystemAvailabilityStatus.Available):
                await systemRegistryUseCaseHandler.MarkSystemAvailableAsync(new MarkSystemAvailableCommand { SystemId = systemId }, cancellationToken);
                break;

            case nameof(SystemAvailabilityStatus.Unavailable):
                await systemRegistryUseCaseHandler.MarkSystemUnavailableAsync(new MarkSystemUnavailableCommand { SystemId = systemId }, cancellationToken);
                break;

            default:
                await systemRegistryUseCaseHandler.MarkSystemAvailabilityUnknownAsync(new MarkSystemAvailabilityUnknownCommand { SystemId = systemId }, cancellationToken);
                break;
        }
    }

    private static string DetermineOperationalStatus(IReadOnlyList<RuntimeInstanceDiagnosticsDto> instances)
    {
        if (instances.Count == 0) return nameof(SystemOperationalStatus.Unknown);
        if (instances.Any(instance => string.Equals(instance.Status.State, "dead", StringComparison.OrdinalIgnoreCase))) return nameof(SystemOperationalStatus.Failed);
        if (instances.All(instance => instance.Status.IsRunning)) return nameof(SystemOperationalStatus.Running);
        if (instances.All(instance => string.Equals(instance.Status.State, "exited", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(instance.Status.State, "stopped", StringComparison.OrdinalIgnoreCase))) return nameof(SystemOperationalStatus.Stopped);
        return nameof(SystemOperationalStatus.Unknown);
    }

    private void LogHealthChecks(Guid systemId, IReadOnlyList<RuntimeInstanceDiagnosticsDto> instances)
    {
        foreach (var instance in instances)
        {
            if (instance.HealthCheck.Status == "health-check-not-configured")
            {
                logger.LogDebug("No Docker health check configured for runtime instance {RuntimeResourceId} of system {SystemId}.", instance.Instance.RuntimeResourceId, systemId);
                continue;
            }

            if (!instance.HealthCheck.IsHealthy)
            {
                logger.LogWarning("Docker health check reports {HealthStatus} for runtime instance {RuntimeResourceId} of system {SystemId}.",
                 instance.HealthCheck.Status, instance.Instance.RuntimeResourceId, systemId);
            }
        }
    }
}