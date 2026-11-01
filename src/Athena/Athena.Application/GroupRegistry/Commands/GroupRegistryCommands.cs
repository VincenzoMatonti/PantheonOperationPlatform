namespace Athena.Application.GroupRegistry.Commands;

public class CreateGroupCommand
{
    public required string Name { get; set; }
}

public class RegisterGroupCommand
{
    public required string Name { get; set; }
}

public class UnregisterGroupCommand
{
    public Guid GroupId { get; set; }
}

public class RenameGroupCommand
{
    public Guid GroupId { get; set; }
    public required string NewName { get; set; }
}

public class EnableGroupCommand
{
    public Guid GroupId { get; set; }
}

public class DisableGroupCommand
{
    public Guid GroupId { get; set; }
}

public class DeleteGroupCommand
{
    public Guid GroupId { get; set; }
}