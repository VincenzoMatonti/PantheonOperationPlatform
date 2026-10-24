using Athena.Cli.Presentation.Inputs.Systems.Management;
using Athena.Cli.Presentation.Inputs.Systems.Registry.Commands;
using Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry;

public static class SystemRegistryInputHandler
{
    public static void AddSystemCommands(this IConfigurator config)
    {
        config.AddBranch("systems", systems =>
        {
            systems.SetDescription("Manage registered systems.");
            systems.AddBranch("registry", registry =>
            {
                registry.SetDescription("Register, query, and manage system records.");

                // Queries
                registry.AddCommand<ListSystemsCommand>("list").WithDescription("List all registered systems.");
                registry.AddCommand<GetSystemCommand>("get").WithDescription("Get a system by ID.");
                registry.AddCommand<GetSystemByNameCommand>("get-by-name").WithDescription("Get a system by name.");
                registry.AddCommand<GetSystemsByRegistrationStatusCommand>("by-registration-status").WithDescription("List systems by registration status.");
                registry.AddCommand<GetSystemsByAvailabilityStatusCommand>("by-availability-status").WithDescription("List systems by availability status.");
                registry.AddCommand<GetSystemsByCapabilityCommand>("by-capability").WithDescription("List systems by capability.");
                registry.AddCommand<GetSystemsByDeletionStatusCommand>("by-deletion-status").WithDescription("List systems by deletion status.");
                registry.AddCommand<GetDeletedSystemsCommand>("deleted").WithDescription("List deleted systems.");
                registry.AddCommand<GetSystemEndpointsCommand>("endpoints").WithDescription("List endpoints for a system.");
                registry.AddCommand<GetSystemCapabilitiesCommand>("capabilities").WithDescription("List capabilities for a system.");

                // Commands
                registry.AddCommand<CreateSystemCommand>("create").WithDescription("Create a system.");
                registry.AddCommand<RegisterSystemCommand>("register").WithDescription("Register a system.");
                registry.AddCommand<UnregisterSystemCommand>("unregister").WithDescription("Unregister a system.");
                registry.AddCommand<RenameSystemCommand>("rename").WithDescription("Rename a system.");
                registry.AddCommand<EnableSystemCommand>("enable").WithDescription("Enable a system.");
                registry.AddCommand<DisableSystemCommand>("disable").WithDescription("Disable a system.");
                registry.AddCommand<DeleteSystemCommand>("delete").WithDescription("Delete a system.");
            });

            // Management is a sibling of registry.
            systems.AddSystemManagementCommands();
        });
    }
}
