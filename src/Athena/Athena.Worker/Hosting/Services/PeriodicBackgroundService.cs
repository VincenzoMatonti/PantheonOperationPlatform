using Athena.Worker.Hosting.Contracts;
using Microsoft.Extensions.Options;

namespace Athena.Worker.Hosting.Services;

public abstract class PeriodicBackgroundService<TOptions>(IOptions<TOptions> options, ILogger logger) : BackgroundService where TOptions : class, IBackgroundTaskOptions
{
    private readonly TOptions _options = options.Value;

    protected abstract Task ExecuteCycleAsync(CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("{BackgroundTask} is disabled.", GetType().Name);
            return;
        }

        if (_options.Interval <= TimeSpan.Zero) throw new InvalidOperationException($"The interval for {GetType().Name} must be greater than zero.");
        logger.LogInformation("{BackgroundTask} started. Interval: {Interval}.", GetType().Name, _options.Interval);
        using var timer = new PeriodicTimer(_options.Interval);
        try
        {
            do
            {
                await ExecuteCycleSafelyAsync(stoppingToken);
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Expected when the host is stopping.
        }
        finally
        {
            logger.LogInformation("{BackgroundTask} stopped.", GetType().Name);
        }
    }

    private async Task ExecuteCycleSafelyAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteCycleAsync(cancellationToken);
            logger.LogDebug("{BackgroundTask} cycle completed.", GetType().Name);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{BackgroundTask} cycle failed.", GetType().Name);
        }
    }
}
