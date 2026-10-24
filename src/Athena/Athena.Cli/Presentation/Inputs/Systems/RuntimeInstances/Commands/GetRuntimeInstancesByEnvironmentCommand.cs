using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public sealed class GetRuntimeInstancesByEnvironmentCommand(SystemRuntimeInstancesService service)
    : AsyncCommand<GetRuntimeInstancesByEnvironmentCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        [CommandArgument(1, "<ENVIRONMENT>")]
        public string Environment { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(Environment)) return ValidationResult.Error("Environment cannot be empty.");
            if (Environment.Length > 100) return ValidationResult.Error("Environment cannot exceed 100 characters.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var instances = await service.GetBySystemIdAndEnvironmentAsync(settings.SystemId, settings.Environment, cancellationToken);
        SystemRuntimeInstanceOutput.RenderBySystemAndEnvironment(settings.SystemId, settings.Environment, instances);
        return 0;
    }
}
