using Athena.Cli.Presentation.Inputs.Common;
using Athena.Cli.Presentation.Interactives.Systems;
using Athena.Cli.Presentation.Outputs.Systems;

namespace Athena.Cli.Presentation.Interactives.Common;

public class CliInteractive(SystemRegistryInteractive systemRegistry)
{
    public async Task RunAsync(CliInput input, CancellationToken cancellationToken = default)
    {
        var args = input.Arguments;
        if (args.Length == 0)
        {
            WriteHelp();
            return;
        }

        if (!string.Equals(args[0], "systems", StringComparison.OrdinalIgnoreCase))
        {
            SystemRegistryOutput.WriteError($"Unknown command: {args[0]}");
            WriteHelp();
            return;
        }

        if (args.Length < 2)
        {
            WriteSystemsHelp();
            return;
        }

        switch (args[1].ToLowerInvariant())
        {
            case "list":
                await systemRegistry.ListAsync(cancellationToken);
                break;

            case "get" when args.Length >= 3 && Guid.TryParse(args[2], out var id):
                await systemRegistry.GetByIdAsync(id, cancellationToken);
                break;

            case "get-by-name" when args.Length >= 3:
                await systemRegistry.GetByNameAsync(string.Join(' ', args.Skip(2)), cancellationToken);
                break;

            case "register":
                if (!TryGetOption(args, "--name", out var name))
                {
                    SystemRegistryOutput.WriteError("Required option: --name");
                    return;
                }

                await systemRegistry.RegisterAsync(name, cancellationToken);
                break;

            case "create":
                if (!TryGetOption(args, "--name", out var createName))
                {
                    SystemRegistryOutput.WriteError("Required option: --name");
                    return;
                }

                await systemRegistry.CreateAsync(createName, cancellationToken);
                break;

            case "enable" when TryGetId(args, out var enableId):
                await systemRegistry.EnableAsync(enableId, cancellationToken);
                break;

            case "disable" when TryGetId(args, out var disableId):
                await systemRegistry.DisableAsync(disableId, cancellationToken);
                break;

            case "unregister" when TryGetId(args, out var unregisterId):
                await systemRegistry.UnregisterAsync(unregisterId, cancellationToken);
                break;

            case "delete" when TryGetId(args, out var deleteId):
                await systemRegistry.DeleteAsync(deleteId, cancellationToken);
                break;

            default:
                SystemRegistryOutput.WriteError("Unknown or incomplete systems command.");
                WriteSystemsHelp();
                break;
        }
    }

    private static bool TryGetOption(string[] args, string option, out string value)
    {
        var index = Array.FindIndex(args, arg => string.Equals(arg, option, StringComparison.OrdinalIgnoreCase));
        if (index >= 0 && index + 1 < args.Length && !args[index + 1].StartsWith("--", StringComparison.Ordinal))
        {
            value = string.Join(' ', args.Skip(index + 1));
            return !string.IsNullOrWhiteSpace(value);
        }

        value = string.Empty;
        return false;
    }

    private static bool TryGetId(string[] args, out Guid id)
    {
        id = Guid.Empty;
        return args.Length >= 3 && Guid.TryParse(args[2], out id);
    }

    private static void WriteHelp()
    {
        SystemRegistryOutput.WriteMessage(
            """
            Pantheon Athena CLI

            Usage:
              systems <command>

            Use 'systems help' to list system commands.
            """);
    }

    private static void WriteSystemsHelp()
    {
        SystemRegistryOutput.WriteMessage(
            """
            System Registry commands

              systems list
              systems get <id>
              systems get-by-name <name>
              systems register --name <name>
              systems create --name <name>
              systems enable <id>
              systems disable <id>
              systems unregister <id>
              systems delete <id>
            """);
    }
}
