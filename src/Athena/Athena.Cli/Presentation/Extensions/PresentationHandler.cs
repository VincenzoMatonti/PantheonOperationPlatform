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
            Description = new DescriptionStyle { Header = "bold white on rgb(35,45,60)" },
            Usage = new UsageStyle
            {
                Header = "bold white on rgb(35,45,60)",
                Options = "grey",
                Command = "bold white",
                CurrentCommand = "bold cyan",
                RequiredArgument = "bold springgreen3",
                OptionalArgument = "grey70"
            },

            Examples = new ExampleStyle { Header = "bold cyan", Arguments = "springgreen3" },
            Arguments = new ArgumentStyle { Header = "bold cyan", RequiredArgument = "bold springgreen3", OptionalArgument = "grey70" },
            Options = new OptionStyle
            {
                Header = "bold cyan",
                DefaultValueHeader = "grey",
                DefaultValue = "grey70",
                RequiredOption = "bold cyan",
                RequiredOptionValue = "bold springgreen3",
                OptionalOptionValue = "grey70"
            },

            Commands = new CommandStyle
            {
                Header = "bold cyan",
                ChildCommand = "bold white",
                RequiredArgument = "springgreen3"
            }
        };

        config.SetHelpProvider(new AthenaHelpProvider(config.Settings));
        return config;
    }

}