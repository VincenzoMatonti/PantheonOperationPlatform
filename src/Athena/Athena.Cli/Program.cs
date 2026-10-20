using Athena.Cli.Infrastructure.Dependencies;
using Athena.Cli.Presentation.Inputs.Common;
using Athena.Cli.Presentation.Interactives.Common;
using Microsoft.Extensions.DependencyInjection;

var baseAddress = new Uri(Environment.GetEnvironmentVariable("ATHENA_API_URL") ?? "http://localhost:8080/");
var services = new ServiceCollection();

services.AddAthenaCli(baseAddress);
using var provider = services.BuildServiceProvider();

try
{
    var input = new CliInput(args);
    var interactive = provider.GetRequiredService<CliInteractive>();
    await interactive.RunAsync(input);
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Error: {exception.Message}");
    Environment.ExitCode = 1;
}
