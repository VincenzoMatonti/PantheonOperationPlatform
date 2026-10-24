namespace Athena.Api.SystemManagement.Requests;

public class ReadRuntimeLogsRequest
{
    public int Tail { get; set; } = 100;

    public DateTime? SinceUtc { get; set; }

    public DateTime? UntilUtc { get; set; }
}