using Athena.Application.BackgroundTasks.Synchronization.Interfaces;
using Athena.Application.SystemRegistry.Dtos;
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
            var isRegistered = !system.IsDeleted
                && string.Equals(system.RegistrationStatus, "Registered", StringComparison.OrdinalIgnoreCase);
            endpointCheckSchedule.PruneObsoleteEndpoints(
                system.Id,
                isRegistered ? system.Endpoints : Array.Empty<SystemEndpointDto>());
            if (!isRegistered)
                continue;

            try
            {
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
                        logger.LogWarning(
                            exception,
                            "Endpoint synchronization phase {Phase} failed for endpoint {EndpointName} of system {SystemId}. Cause: {Cause}.",
                            "check-endpoint",
                            endpoint.Name,
                            system.Id,
                            exception.GetType().Name);
                    }

                    endpointCheckSchedule.RecordResult(system.Id, endpoint.Name, endpoint.BaseAddress, isAvailable, DateTime.UtcNow);
                }

                var availabilityStatus = endpointCheckSchedule.DetermineAvailability(system.Id, system.Endpoints);
                if (string.Equals(system.AvailabilityStatus, availabilityStatus, StringComparison.OrdinalIgnoreCase))
                    continue;

                await UpdateAvailabilityStatusAsync(system.Id, availabilityStatus, cancellationToken);
                logger.LogInformation("System {SystemId} endpoint availability changed from {PreviousStatus} to {CurrentStatus}.",
                    system.Id, system.AvailabilityStatus, availabilityStatus);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Endpoint synchronization failed for system {SystemId} during phase {Phase}; continuing with the next system.",
                    system.Id, "update-availability");
            }
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
