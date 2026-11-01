namespace Athena.Domain.System.Registry.Enums;

public enum SystemInstanceOperationalStatus
{
    Unknown = 0,
    Running = 1,
    Stopped = 2,
    Failed = 3,
    Starting = 4,
    Stopping = 5,
    Unreachable = 6
}
