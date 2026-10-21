using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Commands;

public class DeleteSystemLifecycleOperationCommand(SystemManagementService service) : AsyncCommand<DeleteSystemLifecycleOperationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATION_ID>")]
        public Guid OperationId { get; init; }

        public override ValidationResult Validate() =>
        OperationId == Guid.Empty ? ValidationResult.Error("Operation ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        await service.DeleteOperationAsync(settings.OperationId, cancellationToken);
        SystemManagementOutput.RenderSuccess($"Lifecycle operation '{settings.OperationId}' deleted.");
        return 0;
    }
}
