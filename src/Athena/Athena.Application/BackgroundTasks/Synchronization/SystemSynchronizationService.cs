using System.Text.Json;
using Athena.Application.SystemInstanceManagement.Exceptions;
using Athena.Application.SystemInstanceManagement.Services;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Repositories;
using Microsoft.Extensions.Logging;

namespace Athena.Application.BackgroundTasks.Synchronization;

public sealed class SystemSynchronizationService(
    ISystemInstanceRepository systemInstanceRepository,
    SystemInstanceProcessManagementService processManagementService,
    ILogger<SystemSynchronizationService> logger)
{
    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var instances = await systemInstanceRepository.GetByRegistrationStatusAsync(
            SystemInstanceRegistrationStatus.Registered,
            cancellationToken);

        foreach (var instance in instances)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (instance.IsDeleted || instance.RegistrationStatus != SystemInstanceRegistrationStatus.Registered)
                continue;

            try
            {
                var changed = await processManagementService.SynchronizeStatusAsync(instance.Id.Value, cancellationToken);
                if (changed)
                    logger.LogInformation("SystemInstance {SystemInstanceId} observed operational state was synchronized.", instance.Id.Value);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
            {
                var unobservableStatus = GetUnobservableStatus(exception, cancellationToken);
                if (unobservableStatus is not null)
                {
                    try
                    {
                        var changed = await processManagementService.MarkStatusUnobservableAsync(
                            instance.Id.Value,
                            unobservableStatus.Value,
                            cancellationToken);
                        if (changed)
                        {
                            logger.LogWarning(
                                exception,
                                "SystemInstance {SystemInstanceId} synchronization phase {Phase} changed operational status to {OperationalStatus}. Cause: {Cause}.",
                                instance.Id.Value,
                                "observe-status",
                                unobservableStatus.Value,
                                exception.GetType().Name);
                        }
                        else
                        {
                            logger.LogDebug(
                                exception,
                                "SystemInstance {SystemInstanceId} synchronization phase {Phase} could not observe status; status was unchanged or an operation is active. Cause: {Cause}.",
                                instance.Id.Value,
                                "observe-status",
                                exception.GetType().Name);
                        }
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception persistenceException) when (!cancellationToken.IsCancellationRequested)
                    {
                        logger.LogError(
                            persistenceException,
                            "SystemInstance {SystemInstanceId} synchronization phase {Phase} failed to persist an unobservable status after {Cause}.",
                            instance.Id.Value,
                            "persist-unobservable-status",
                            exception.GetType().Name);
                    }
                }
                else
                {
                    logger.LogDebug(
                        exception,
                        "SystemInstance {SystemInstanceId} synchronization phase {Phase} failed; continuing with the next instance. Cause: {Cause}.",
                        instance.Id.Value,
                        "observe-status",
                        exception.GetType().Name);
                }
            }
        }
    }

    private static SystemInstanceOperationalStatus? GetUnobservableStatus(
        Exception exception,
        CancellationToken cancellationToken)
    {
        if ((exception is OperationCanceledException && !cancellationToken.IsCancellationRequested)
            || exception is TimeoutException
            || exception is HttpRequestException { StatusCode: null })
            return SystemInstanceOperationalStatus.Unreachable;

        if (exception is ExecutionAgentNotConfiguredException
            or ExecutionAgentResponseException
            or HttpRequestException
            or JsonException)
            return SystemInstanceOperationalStatus.Unknown;

        return null;
    }
}
