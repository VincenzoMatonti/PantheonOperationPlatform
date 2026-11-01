using Athena.Application.SystemManagement.Contracts;
using Athena.Application.SystemManagement.Services;
using Microsoft.Extensions.Logging;

namespace Athena.Application.BackgroundTasks.Synchronization;

public sealed class SystemSynchronizationService(
    SystemInstanceProcessManagementService processManagementService,
    ILogger<SystemSynchronizationService> logger)
{
    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var instances = await processManagementService.GetAllAsync(cancellationToken);
        foreach (var instance in instances)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var status = await processManagementService.GetStatusAsync(instance.Id, cancellationToken);
                logger.LogDebug("SystemInstance {SystemInstanceId} is {OperationalState}.", instance.Id, status.State);
            }
            catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
            {
                if (exception is HttpRequestException
                    or TaskCanceledException
                    or ExecutionAgentNotConfiguredException
                    or ExecutionAgentResponseException
                    or System.Text.Json.JsonException)
                {
                    try
                    {
                        await processManagementService.MarkUnreachableAsync(instance.Id, cancellationToken);
                    }
                    catch (Exception persistenceException) when (!cancellationToken.IsCancellationRequested)
                    {
                        logger.LogError(persistenceException, "Could not persist unreachable status for SystemInstance {SystemInstanceId}.", instance.Id);
                    }
                }

                logger.LogError(exception, "SystemInstance {SystemInstanceId} synchronization failed; continuing with the next instance.", instance.Id);
            }
        }
    }
}
