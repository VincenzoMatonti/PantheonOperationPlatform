using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Queries;

public class GetOrchestrationLifecycleOperationsByTypeCommand(OrchestrationManagementService service)
    : AsyncCommand<GetOrchestrationLifecycleOperationsByTypeCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<TYPE>")]
        public string Type { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(Type) ? ValidationResult.Error("Operation type cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetOperationsByTypeAsync(settings.Type, cancellationToken);
        OrchestrationManagementOutput.WriteOperations(operations);
        return 0;
    }
}
