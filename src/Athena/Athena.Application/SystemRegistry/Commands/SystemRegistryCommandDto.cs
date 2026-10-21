namespace Athena.Application.SystemRegistry.Commands;

public class RegisterSystemCommand
{
    public required string Name { get; set; }
}

public class UnregisterSystemCommand
{
    public Guid SystemId { get; set; }
}

public class RenameSystemCommand
{
    public Guid SystemId { get; set; }
    public required string NewName { get; set; }
}

public class EnableSystemCommand
{
    public Guid SystemId { get; set; }
}

public class DisableSystemCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemAvailableCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemUnavailableCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemAvailabilityUnknownCommand
{
    public Guid SystemId { get; set; }
}

public class AddSystemEndpointCommand
{
    public Guid SystemId { get; set; }
    public required string Name { get; set; }
    public required string BaseAddress { get; set; }
}

public class RemoveSystemEndpointCommand
{
    public Guid SystemId { get; set; }
    public required string EndpointName { get; set; }
}

public class AddSystemCapabilityCommand
{
    public Guid SystemId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class RemoveSystemCapabilityCommand
{
    public Guid SystemId { get; set; }
    public required string CapabilityName { get; set; }
}

public class DeleteSystemCommand
{
    public Guid SystemId { get; set; }
}