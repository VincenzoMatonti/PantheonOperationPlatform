namespace Athena.Application.BlockRegistry.Commands;

public class CreateBlockCommand
{
    public Guid EcosystemId { get; set; }
    public required string Name { get; set; }
}

public class RegisterBlockCommand
{
    public Guid BlockId { get; set; }
}

public class UnregisterBlockCommand
{
    public Guid BlockId { get; set; }
}

public class RenameBlockCommand
{
    public Guid BlockId { get; set; }
    public required string NewName { get; set; }
}

public class EnableBlockCommand
{
    public Guid BlockId { get; set; }
}

public class DisableBlockCommand
{
    public Guid BlockId { get; set; }
}

public class DeleteBlockCommand
{
    public Guid BlockId { get; set; }
}

public class MarkBlockAvailableCommand
{
    public Guid BlockId { get; set; }
}

public class MarkBlockUnavailableCommand
{
    public Guid BlockId { get; set; }
}

public class MarkBlockAvailabilityUnknownCommand
{
    public Guid BlockId { get; set; }
}

public class MarkBlockErrorCommand
{
    public Guid BlockId { get; set; }
}