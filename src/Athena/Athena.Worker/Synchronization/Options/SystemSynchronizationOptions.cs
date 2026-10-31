using Athena.Worker.Hosting.Contracts;

namespace Athena.Worker.Synchronization.Options;

public sealed class SystemSynchronizationOptions : IBackgroundTaskOptions
{
    public const string SectionName = "BackgroundTasks:SystemSynchronization";

    public bool Enabled { get; set; } = true;

    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);
}