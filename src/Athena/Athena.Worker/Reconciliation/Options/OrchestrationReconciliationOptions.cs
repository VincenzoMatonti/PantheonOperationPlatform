using Athena.Worker.Hosting.Contracts;

namespace Athena.Worker.Reconciliation.Options;

public sealed class OrchestrationReconciliationOptions : IBackgroundTaskOptions
{
    public const string SectionName = "BackgroundTasks:Reconciliation";
    public bool Enabled { get; set; } = true;
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);
}