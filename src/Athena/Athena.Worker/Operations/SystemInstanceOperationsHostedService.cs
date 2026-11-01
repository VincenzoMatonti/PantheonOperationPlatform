using Athena.Application.BackgroundTasks.Operations;
using Athena.Worker.Operations.Options;
using Microsoft.Extensions.Options;

namespace Athena.Worker.Operations;

public sealed class SystemInstanceOperationsHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<SystemInstanceOperationsOptions> options,
    ILogger<SystemInstanceOperationsHostedService> logger) : BackgroundService
{
    private readonly SystemInstanceOperationsOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("SystemInstance operation processing is disabled.");
            return;
        }

        var legacy = string.Equals(_options.AgentProtocol, "Legacy", StringComparison.OrdinalIgnoreCase);
        logger.LogInformation("SystemInstance operation processing started with {AgentProtocol} Agent protocol.", _options.AgentProtocol);
        while (!stoppingToken.IsCancellationRequested)
        {
            var processed = false;
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var processor = scope.ServiceProvider.GetRequiredService<SystemInstanceOperationProcessor>();
                processed = await processor.ProcessOneAsync(
                    legacy,
                    _options.LeaseDuration,
                    _options.PollingInterval,
                    stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "SystemInstance operation processing cycle failed.");
            }

            if (!processed)
                await Task.Delay(_options.PollingInterval, stoppingToken);
        }
    }
}
