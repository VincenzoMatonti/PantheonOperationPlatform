using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Commands;

public abstract class SystemLifecycleOperationActionCommand(SystemManagementService service) : AsyncCommand<SystemLifecycleOperationActionCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATION_ID>")]
        public Guid OperationId { get; init; }

        public override ValidationResult Validate() =>
          OperationId == Guid.Empty ? ValidationResult.Error("Operation ID cannot be empty.") : ValidationResult.Success();
    }

    protected abstract Task<SystemLifecycleOperationResponse> ExecuteActionAsync(Guid operationId, CancellationToken cancellationToken);

    protected SystemManagementService Service { get; } = service;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operation = await ExecuteActionAsync(settings.OperationId, cancellationToken);
        SystemManagementOutput.RenderOperation(operation);
        return 0;
    }
}
