using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Diagnostic.Commands;

public class ReadRuntimeLogsCommand(SystemDiagnosticService service)    : AsyncCommand<ReadRuntimeLogsCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<RUNTIME_INSTANCE_ID>")]
        public Guid RuntimeInstanceId { get; init; }

        [CommandOption("--tail <COUNT>")]
        public int Tail { get; init; } = 100;

        [CommandOption("--since <UTC>")]
        public DateTime? SinceUtc { get; init; }

        [CommandOption("--until <UTC>")]
        public DateTime? UntilUtc { get; init; }

        public override ValidationResult Validate()
        {
            if (RuntimeInstanceId == Guid.Empty) return ValidationResult.Error("Runtime instance ID cannot be empty.");
            if (Tail <= 0) return ValidationResult.Error("--tail must be greater than zero.");
            if (SinceUtc.HasValue && UntilUtc.HasValue && SinceUtc > UntilUtc) return ValidationResult.Error("--since cannot be later than --until.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var logs = await service.ReadRuntimeLogsAsync(settings.RuntimeInstanceId, settings.Tail, settings.SinceUtc, settings.UntilUtc, cancellationToken);
        SystemDiagnosticOutput.RenderLogs(logs);
        return 0;
    }
}