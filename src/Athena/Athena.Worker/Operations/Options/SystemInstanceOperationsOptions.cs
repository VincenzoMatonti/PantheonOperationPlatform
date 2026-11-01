namespace Athena.Worker.Operations.Options;

public sealed class SystemInstanceOperationsOptions
{
    public const string SectionName = "BackgroundTasks:SystemInstanceOperations";
    public bool Enabled { get; set; } = true;
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(2);
    public TimeSpan LeaseDuration { get; set; } = TimeSpan.FromMinutes(1);
    public string AgentProtocol { get; set; } = "Correlated";
}
