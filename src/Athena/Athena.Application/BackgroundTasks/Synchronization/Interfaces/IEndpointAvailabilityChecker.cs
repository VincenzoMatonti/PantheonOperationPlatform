namespace Athena.Application.BackgroundTasks.Synchronization.Interfaces;

public interface IEndpointAvailabilityChecker
{
    Task<bool> CheckAvailabilityAsync(string baseAddress, CancellationToken cancellationToken = default);
}