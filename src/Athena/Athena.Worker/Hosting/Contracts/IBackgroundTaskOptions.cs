namespace Athena.Worker.Hosting.Contracts;

public interface IBackgroundTaskOptions
{
    bool Enabled { get; set; }

    TimeSpan Interval { get; set; }
}