using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public class GetRuntimeInstancesByResourceNameCommand(SystemRuntimeInstancesService service)
    : AsyncCommand<GetRuntimeInstancesByResourceNameCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<RUNTIME_RESOURCE_NAME>")]
        public string RuntimeResourceName { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(RuntimeResourceName)) return ValidationResult.Error("Runtime resource name cannot be empty.");
            if (RuntimeResourceName.Length > 255) return ValidationResult.Error("Runtime resource name cannot exceed 255 characters.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var instances = await service.GetByRuntimeResourceNameAsync(settings.RuntimeResourceName, cancellationToken);
        SystemRuntimeInstanceOutput.RenderByResourceName(settings.RuntimeResourceName, instances);
        return 0;
    }
}
