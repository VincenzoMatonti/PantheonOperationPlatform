using Athena.Cli.Presentation.Inputs.Orchestrations.Management;
using Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Commands;
using Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Queries;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry;

public static class OrchestrationRegistryInputHandler
{
    public static void AddOrchestrationCommands(this IConfigurator config)
    {
        config.AddBranch("orchestrations", orchestrations =>
        {
            orchestrations.SetDescription("Manage registered orchestrations.");
            orchestrations.AddBranch("registry", registry =>
            {
                registry.SetDescription("Manage the orchestration registry.");

                // Queries
                registry.AddCommand<ListOrchestrationsCommand>("list").WithDescription("List all orchestrations.");
                registry.AddCommand<GetOrchestrationCommand>("get").WithDescription("Get an orchestration by ID.");
                registry.AddCommand<GetOrchestrationByNameCommand>("get-by-name").WithDescription("Get an orchestration by name.");
                registry.AddCommand<GetOrchestrationsByRegistrationStatusCommand>("by-registration-status").WithDescription("List orchestrations by registration status.");
                registry.AddCommand<GetOrchestrationsByAvailabilityStatusCommand>("by-availability-status").WithDescription("List orchestrations by availability status.");
                registry.AddCommand<GetDeletedOrchestrationsCommand>("deleted").WithDescription("List deleted orchestrations.");

                // Commands
                registry.AddCommand<CreateOrchestrationCommand>("create").WithDescription("Create an orchestration.");
                registry.AddCommand<RegisterOrchestrationCommand>("register").WithDescription("Register an orchestration.");
                registry.AddCommand<UnregisterOrchestrationCommand>("unregister").WithDescription("Unregister an orchestration.");
                registry.AddCommand<RenameOrchestrationCommand>("rename").WithDescription("Rename an orchestration.");
                registry.AddCommand<EnableOrchestrationCommand>("enable").WithDescription("Enable an orchestration.");
                registry.AddCommand<DisableOrchestrationCommand>("disable").WithDescription("Disable an orchestration.");
                registry.AddCommand<DeleteOrchestrationCommand>("delete").WithDescription("Delete an orchestration.");

                // Availability
                registry.AddBranch("availability", availability =>
                {
                    availability.SetDescription("Manage orchestration availability.");
                    availability.AddCommand<MarkOrchestrationAvailableCommand>("available").WithDescription("Mark an orchestration as available.");
                    availability.AddCommand<MarkOrchestrationUnavailableCommand>("unavailable").WithDescription("Mark an orchestration as unavailable.");
                    availability.AddCommand<MarkOrchestrationAvailabilityUnknownCommand>("unknown").WithDescription("Mark orchestration availability as unknown.");
                });
            });

            // Management: sibling of registry
            orchestrations.AddOrchestrationManagementCommands();
        });
    }
}