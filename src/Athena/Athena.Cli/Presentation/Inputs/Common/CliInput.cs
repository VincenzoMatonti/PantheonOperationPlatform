namespace Athena.Cli.Presentation.Inputs.Common;

public class CliInput(string[] args)
{
    public string[] Arguments { get; } = args;
}
