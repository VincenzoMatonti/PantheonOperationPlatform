using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace Athena.Cli.Presentation.Help;

public class AthenaHelpProvider(ICommandAppSettings settings) : HelpProvider(settings)
{


    public override IEnumerable<IRenderable> GetHeader(ICommandModel model, ICommandInfo? command)
    {
        yield return new FigletText("ATHENA").Color(Color.Cyan);
        yield return new Text(Environment.NewLine);
        yield return new Markup("[bold white]PANTHEON OPERATION PLATFORM[/]");
        yield return new Text(Environment.NewLine);
        yield return new Markup("[dim]Systems orchestration · Control plane · CLI[/]");
        yield return new Text(Environment.NewLine);
        yield return new Markup("[bold cyan]ATHENA CLI[/] [dim]— COMMAND REFERENCE[/]");
        yield return new Text(Environment.NewLine);
        yield return new Text(Environment.NewLine);
        yield return new Text(Environment.NewLine);
    }


    public override IEnumerable<IRenderable> GetDescription(ICommandModel model, ICommandInfo? command)
    {
        return WithDividers(base.GetDescription(model, command));
    }

    public override IEnumerable<IRenderable> GetUsage(ICommandModel model, ICommandInfo? command)
    {
        return WithDividers(base.GetUsage(model, command));
    }

    public override IEnumerable<IRenderable> GetOptions(ICommandModel model, ICommandInfo? command)
    {
        return WithDividers(base.GetOptions(model, command));
    }

    public override IEnumerable<IRenderable> GetCommands(ICommandModel model, ICommandInfo? command)
    {
        return WithDividers(base.GetCommands(model, command));
    }


    private static IEnumerable<IRenderable> WithDividers(IEnumerable<IRenderable> content)
    {
        foreach (var item in content)
        {
            yield return item;
        }

        yield return new Text(Environment.NewLine);
    }

}
