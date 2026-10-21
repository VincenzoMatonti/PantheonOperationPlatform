using Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Commands;
using Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Queries;
using Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Commands;
using Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Queries;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management;

public static class SystemManagementInputHandler
{
    public static IConfigurator<CommandSettings> AddSystemManagementCommands(this IConfigurator<CommandSettings> config)
    {
        config.AddBranch("management", management =>
        {
            management.SetDescription("Manage system lifecycles and lifecycle operations.");
            management.AddBranch("lifecycles", lifecycles =>
            {
                lifecycles.SetDescription("Query and manage system lifecycles.");
                lifecycles.AddCommand<ListSystemLifecyclesCommand>("list");
                lifecycles.AddCommand<GetSystemLifecycleCommand>("get");
                lifecycles.AddCommand<GetSystemLifecyclesByDesiredStateCommand>("by-desired-state");
                lifecycles.AddCommand<GetSystemLifecyclesByOperationalStatusCommand>("by-operational-status");
                lifecycles.AddCommand<GetSystemLifecyclesByDeletionStatusCommand>("by-deletion-status");
                lifecycles.AddCommand<ListDeletedSystemLifecyclesCommand>("deleted");
                lifecycles.AddCommand<CreateSystemLifecycleCommand>("create");
                lifecycles.AddCommand<SetSystemDesiredStateCommand>("set-desired-state");
                lifecycles.AddCommand<MarkSystemStartingCommand>("starting");
                lifecycles.AddCommand<MarkSystemRunningCommand>("running");
                lifecycles.AddCommand<MarkSystemStoppingCommand>("stopping");
                lifecycles.AddCommand<MarkSystemStoppedCommand>("stopped");
                lifecycles.AddCommand<MarkSystemFailedCommand>("failed");
                lifecycles.AddCommand<MarkSystemUnknownCommand>("unknown");
                lifecycles.AddCommand<DeleteSystemLifecycleCommand>("delete");
            });

            management.AddBranch("operations", operations =>
            {
                operations.SetDescription("Query and manage lifecycle operations.");
                operations.AddCommand<GetSystemLifecycleOperationCommand>("get");
                operations.AddCommand<ListSystemLifecycleOperationsCommand>("list");
                operations.AddCommand<GetSystemLifecycleOperationsByStatusCommand>("by-status");
                operations.AddCommand<GetSystemLifecycleOperationsBySystemIdAndStatusCommand>("by-system-and-status");
                operations.AddCommand<GetSystemLifecycleOperationsByDeletionStatusCommand>("by-deletion-status");
                operations.AddCommand<ListDeletedSystemLifecycleOperationsCommand>("deleted");
                operations.AddCommand<CreateSystemLifecycleOperationCommand>("create");
                operations.AddCommand<StartSystemLifecycleOperationCommand>("start");
                operations.AddCommand<CompleteSystemLifecycleOperationCommand>("complete");
                operations.AddCommand<FailSystemLifecycleOperationCommand>("fail");
                operations.AddCommand<CancelSystemLifecycleOperationCommand>("cancel");
                operations.AddCommand<DeleteSystemLifecycleOperationCommand>("delete");
            });
        });

        return config;
    }
}
