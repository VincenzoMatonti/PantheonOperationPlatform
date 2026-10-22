using Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Commands;
using Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Queries;
using Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Commands;
using Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Queries;
using Athena.Cli.Presentation.Inputs.Orchestrations.Management.OrchestrationSystems.Commands;
using Athena.Cli.Presentation.Inputs.Orchestrations.Management.OrchestrationSystems.Queries;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management;

public static class OrchestrationManagementInputHandler
{
    public static IConfigurator<CommandSettings> AddOrchestrationManagementCommands(this IConfigurator<CommandSettings> config)
    {
        config.AddBranch("management", management =>
        {
            management.SetDescription("Manage orchestration lifecycles and lifecycle operations.");
            management.AddBranch("lifecycle", lifecycle =>
            {
                lifecycle.SetDescription("Query and manage orchestration lifecycles.");

                // Queries
                lifecycle.AddCommand<GetOrchestrationLifecycleCommand>("get");
                lifecycle.AddCommand<ListOrchestrationLifecyclesCommand>("list");
                lifecycle.AddCommand<GetOrchestrationLifecyclesByDesiredStateCommand>("by-desired-state");
                lifecycle.AddCommand<GetOrchestrationLifecyclesByOperationalStatusCommand>("by-operational-status");
                lifecycle.AddCommand<GetDeletedOrchestrationLifecyclesCommand>("deleted");

                // Commands
                lifecycle.AddCommand<CreateOrchestrationLifecycleCommand>("create");
                lifecycle.AddCommand<SetOrchestrationDesiredStateCommand>("set-desired-state");
                lifecycle.AddCommand<MarkOrchestrationStartingCommand>("starting");
                lifecycle.AddCommand<MarkOrchestrationRunningCommand>("running");
                lifecycle.AddCommand<MarkOrchestrationStoppingCommand>("stopping");
                lifecycle.AddCommand<MarkOrchestrationStoppedCommand>("stopped");
                lifecycle.AddCommand<MarkOrchestrationFailedCommand>("failed");
                lifecycle.AddCommand<MarkOrchestrationOperationalStatusUnknownCommand>("unknown");
                lifecycle.AddCommand<DeleteOrchestrationLifecycleCommand>("delete");
            });

            management.AddBranch("operations", operations =>
            {
                operations.SetDescription("Query and manage orchestration lifecycle operations.");

                // Queries
                operations.AddCommand<GetOrchestrationLifecycleOperationCommand>("get");
                operations.AddCommand<GetOrchestrationLifecycleOperationsCommand>("by-orchestration");
                operations.AddCommand<ListOrchestrationLifecycleOperationsCommand>("list");
                operations.AddCommand<GetOrchestrationLifecycleOperationsByTypeCommand>("by-type");
                operations.AddCommand<GetOrchestrationLifecycleOperationsByStatusCommand>("by-status");
                operations.AddCommand<GetOrchestrationLifecycleOperationsByOrchestrationAndStatusCommand>("by-orchestration-and-status");
                operations.AddCommand<GetPendingOrchestrationLifecycleOperationsCommand>("pending");
                operations.AddCommand<GetRunningOrchestrationLifecycleOperationsCommand>("running");
                operations.AddCommand<GetFailedOrchestrationLifecycleOperationsCommand>("failed");
                operations.AddCommand<GetDeletedOrchestrationLifecycleOperationsCommand>("deleted");

                // Commands
                operations.AddCommand<CreateOrchestrationLifecycleOperationCommand>("create");
                operations.AddCommand<StartOrchestrationLifecycleOperationCommand>("start");
                operations.AddCommand<CompleteOrchestrationLifecycleOperationCommand>("complete");
                operations.AddCommand<FailOrchestrationLifecycleOperationCommand>("fail");
                operations.AddCommand<CancelOrchestrationLifecycleOperationCommand>("cancel");
                operations.AddCommand<DeleteOrchestrationLifecycleOperationCommand>("delete");
            });

            management.AddBranch("orchestration-system", systems =>
            {
                systems.SetDescription("Query and manage orchestration-system associations.");

                // Queries
                systems.AddCommand<GetOrchestrationSystemCommand>("get");
                systems.AddCommand<ListOrchestrationSystemsCommand>("list");

                // Commands
                systems.AddCommand<CreateOrchestrationSystemCommand>("create");
                systems.AddCommand<DeleteOrchestrationSystemCommand>("delete");
                systems.AddCommand<RestoreOrchestrationSystemCommand>("restore");
            });
        });

        return config;
    }
}