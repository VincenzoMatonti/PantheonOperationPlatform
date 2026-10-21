using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Queries;

public class GetSystemLifecycleOperationsByStatusCommand(SystemManagementService service) : AsyncCommand<GetSystemLifecycleOperationsByStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<STATUS>")]
        public string Status { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(Status) ? ValidationResult.Error("Operation status is required.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetOperationsByStatusAsync(settings.Status, cancellationToken);
        SystemManagementOutput.RenderOperations(operations);
        return 0;
    }
}
