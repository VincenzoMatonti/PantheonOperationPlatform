using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Diagnostic.Commands;

public class CheckRuntimeHealthCommand(SystemDiagnosticService service) : AsyncCommand<CheckRuntimeHealthCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<RUNTIME_INSTANCE_ID>")]
        public Guid RuntimeInstanceId { get; init; }

        public override ValidationResult Validate() =>
            RuntimeInstanceId == Guid.Empty ? ValidationResult.Error("Runtime instance ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var health = await service.CheckRuntimeHealthAsync(settings.RuntimeInstanceId, cancellationToken);
        SystemDiagnosticOutput.RenderHealthCheck(health);
        return health.IsHealthy ? 0 : 1;
    }
}