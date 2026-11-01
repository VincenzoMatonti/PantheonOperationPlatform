namespace Athena.Application.EcosystemRegistry.Commands;

public class CreateEcosystemCommand
{
    public Guid CompanyId { get; set; }
    public required string Name { get; set; }
}

public class RegisterEcosystemCommand
{
    public Guid EcosystemId { get; set; }
}

public class UnregisterEcosystemCommand
{
    public Guid EcosystemId { get; set; }
}

public class RenameEcosystemCommand
{
    public Guid EcosystemId { get; set; }
    public required string NewName { get; set; }
}

public class EnableEcosystemCommand
{
    public Guid EcosystemId { get; set; }
}

public class DisableEcosystemCommand
{
    public Guid EcosystemId { get; set; }
}

public class DeleteEcosystemCommand
{
    public Guid EcosystemId { get; set; }
}

public class MarkEcosystemAvailableCommand
{
    public Guid EcosystemId { get; set; }
}

public class MarkEcosystemUnavailableCommand
{
    public Guid EcosystemId { get; set; }
}

public class MarkEcosystemAvailabilityUnknownCommand
{
    public Guid EcosystemId { get; set; }
}

public class MarkEcosystemErrorCommand
{
    public Guid EcosystemId { get; set; }
}