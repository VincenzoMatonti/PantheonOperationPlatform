using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public class RegisterRuntimeInstanceCommand(SystemRuntimeInstancesService service) : AsyncCommand<RegisterRuntimeInstanceCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--system-id <SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        [CommandOption("--provider <PROVIDER>")]
        public string RuntimeProvider { get; init; } = string.Empty;

        [CommandOption("--resource-id <RESOURCE_ID>")]
        public string RuntimeResourceId { get; init; } = string.Empty;

        [CommandOption("--resource-name <RESOURCE_NAME>")]
        public string RuntimeResourceName { get; init; } = string.Empty;

        [CommandOption("--environment <ENVIRONMENT>")]
        public string Environment { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(RuntimeProvider)) return ValidationResult.Error("Runtime provider is required.");
            if (RuntimeProvider.Length > 100) return ValidationResult.Error("Runtime provider cannot exceed 100 characters.");
            if (string.IsNullOrWhiteSpace(RuntimeResourceId)) return ValidationResult.Error("Runtime resource ID is required.");
            if (RuntimeResourceId.Length > 255) return ValidationResult.Error("Runtime resource ID cannot exceed 255 characters.");
            if (string.IsNullOrWhiteSpace(RuntimeResourceName)) return ValidationResult.Error("Runtime resource name is required.");
            if (RuntimeResourceName.Length > 255) return ValidationResult.Error("Runtime resource name cannot exceed 255 characters.");
            if (string.IsNullOrWhiteSpace(Environment)) return ValidationResult.Error("Environment is required.");
            if (Environment.Length > 100) return ValidationResult.Error("Environment cannot exceed 100 characters.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var request = new RegisterSystemRuntimeInstanceRequest(
            settings.SystemId, settings.RuntimeProvider, settings.RuntimeResourceId, settings.RuntimeResourceName, settings.Environment);
        var instance = await service.RegisterAsync(request, cancellationToken);
        SystemRuntimeInstanceOutput.RenderRegistered(instance);
        return 0;
    }
}
