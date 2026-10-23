using System.Diagnostics;

namespace Athena.Infrastructure.Services.Client;

internal static class DockerCli
{
    public static async Task<string> ExecuteAsync(IEnumerable<string> arguments, CancellationToken cancellationToken = default)
    {
        var startInfo = CreateStartInfo(arguments);
        using var process = new Process { StartInfo = startInfo };
        process.Start();
        var stdoutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        try
        {
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                // Il processo potrebbe essere già terminato.
            }

            throw;
        }

        var stdout = await stdoutTask;
        var stderr = await stderrTask;
        if (process.ExitCode != 0) throw new InvalidOperationException($"Comando Docker fallito ({process.ExitCode}): " + $"{stderr.Trim()}");
        return stdout;
    }

    public static ProcessStartInfo CreateStartInfo(IEnumerable<string> arguments)
    {
        var info = new ProcessStartInfo
        {
            FileName = "docker",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
            info.ArgumentList.Add(argument);

        return info;
    }
}
