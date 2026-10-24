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
        yield return new Markup("[dim]━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━[/]");
        yield return new Text(Environment.NewLine);
        yield return new Text(Environment.NewLine);
    }
}
