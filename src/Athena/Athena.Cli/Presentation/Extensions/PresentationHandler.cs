using Athena.Cli.Presentation.Help;
using Athena.Cli.Presentation.Inputs.Orchestrations.Registry;
using Athena.Cli.Presentation.Inputs.Systems.Registry;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace Athena.Cli.Presentation.Extensions;

public static class PresentationHandler
{
    public static IConfigurator AddAthenaCliCommands(this IConfigurator config)
    {
        config.AddSystemCommands();
        config.AddOrchestrationCommands();
        return config;
    }

    public static IConfigurator UseAthenaHelp(this IConfigurator config)
    {

        config.Settings.HelpProviderStyles = new HelpProviderStyle
        {
            Description = new DescriptionStyle { Header = "bold yellow" },
            Usage = new UsageStyle
            {
                Header = "bold yellow",
                Options = "dim",
                Command = "bold cyan",
                CurrentCommand = "bold cyan",
                RequiredArgument = "bold green",
                OptionalArgument = "dim green"
            },

            Examples = new ExampleStyle { Header = "bold yellow", Arguments = "cyan" },
            Arguments = new ArgumentStyle { Header = "bold yellow", RequiredArgument = "bold green", OptionalArgument = "dim green" },
            Options = new OptionStyle
            {
                Header = "bold yellow",
                DefaultValueHeader = "dim",
                DefaultValue = "dim",
                RequiredOption = "bold cyan",
                RequiredOptionValue = "bold green",
                OptionalOptionValue = "dim green"
            },

            Commands = new CommandStyle
            {
                Header = "bold yellow",
                ChildCommand = "bold cyan",
                RequiredArgument = "bold green"
            }
        };

        config.SetHelpProvider(new AthenaHelpProvider(config.Settings));
        return config;
    }
}