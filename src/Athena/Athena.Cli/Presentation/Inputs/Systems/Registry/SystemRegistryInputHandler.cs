using Athena.Cli.Presentation.Inputs.Systems.Management;
using Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;
using Athena.Cli.Presentation.Inputs.Systems.Registry.Commands;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry;

public static class SystemRegistryInputHandler
{
    public static void AddSystemCommands(this IConfigurator config)
    {
        config.AddBranch("systems", systems =>
        {
            systems.SetDescription("Manage registered systems.");
            systems.AddCommand<ListSystemsCommand>("list").WithDescription("List all systems.");
            systems.AddCommand<GetSystemCommand>("get").WithDescription("Get a system by ID.");
            systems.AddCommand<GetSystemByNameCommand>("get-by-name");
            systems.AddCommand<GetSystemsByRegistrationStatusCommand>("by-registration-status");
            systems.AddCommand<GetSystemsByAvailabilityStatusCommand>("by-availability-status");
            systems.AddCommand<GetSystemsByCapabilityCommand>("by-capability");
            systems.AddCommand<GetSystemsByDeletionStatusCommand>("by-deletion-status");
            systems.AddCommand<GetDeletedSystemsCommand>("deleted");
            systems.AddCommand<GetSystemEndpointsCommand>("endpoints");
            systems.AddCommand<GetSystemCapabilitiesCommand>("capabilities");
            systems.AddCommand<CreateSystemCommand>("create");
            systems.AddCommand<RegisterSystemCommand>("register");
            systems.AddCommand<UnregisterSystemCommand>("unregister");
            systems.AddCommand<RenameSystemCommand>("rename");
            systems.AddCommand<EnableSystemCommand>("enable");
            systems.AddCommand<DisableSystemCommand>("disable");
            systems.AddCommand<DeleteSystemCommand>("delete");
            systems.AddSystemManagementCommands();
        });
    }
}