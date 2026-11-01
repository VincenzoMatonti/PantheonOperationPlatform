using Athena.Application.BackgroundTasks.Synchronization.Interfaces;
using Athena.Application.SystemRegistry.Commands;
using Athena.Application.SystemRegistry.UseCases;
using Athena.Domain.System.Registry.Enums;
using Microsoft.Extensions.Logging;

namespace Athena.Application.BackgroundTasks.Synchronization;

public sealed class SystemEndpointSynchronizationService(
    SystemRegistryUseCaseHandler systemRegistryUseCaseHandler,
    IEndpointAvailabilityChecker endpointAvailabilityChecker,
    EndpointCheckSchedule endpointCheckSchedule,
    ILogger<SystemEndpointSynchronizationService> logger)
{
    private static readonly TimeSpan EndpointCheckInterval = TimeSpan.FromMinutes(5);

    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var nowUtc = DateTime.UtcNow;
        var systems = await systemRegistryUseCaseHandler.GetAllSystemsAsync(cancellationToken);
        foreach (var system in systems)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (system.IsDeleted || !string.Equals(system.RegistrationStatus, "Registered", StringComparison.OrdinalIgnoreCase))
                continue;

            foreach (var endpoint in system.Endpoints)
            {
                if (!endpointCheckSchedule.ShouldCheck(system.Id, endpoint.Name, endpoint.BaseAddress, nowUtc, EndpointCheckInterval))
                    continue;

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
            if (string.Equals(system.AvailabilityStatus, availabilityStatus, StringComparison.OrdinalIgnoreCase))
                continue;

            await UpdateAvailabilityStatusAsync(system.Id, availabilityStatus, cancellationToken);
            logger.LogInformation("System {SystemId} endpoint availability changed from {PreviousStatus} to {CurrentStatus}.",
                system.Id, system.AvailabilityStatus, availabilityStatus);
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
}
