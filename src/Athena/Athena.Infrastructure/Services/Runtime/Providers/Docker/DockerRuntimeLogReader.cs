using System.Globalization;
using System.Threading.Channels;
using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces.Runtime;
using Athena.Infrastructure.Services.Client;

namespace Athena.Infrastructure.Services.Runtime.Providers.Docker;

public class DockerRuntimeLogReader : IRuntimeLogReader
{
    public string ProviderName => "docker";

    public async IAsyncEnumerable<RuntimeLogEntryDto> ReadLogsAsync(
        RuntimeResourceReferenceDto resource, RuntimeLogOptionsDto options, 
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(resource);
        ArgumentNullException.ThrowIfNull(options);
        if (!string.Equals(resource.Provider, ProviderName, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("La risorsa non appartiene al provider Docker.", nameof(resource));
        if (string.IsNullOrWhiteSpace(resource.ResourceId)) throw new ArgumentException("Il resource ID è obbligatorio.", nameof(resource));
        if (options.Tail < 1) throw new ArgumentOutOfRangeException(nameof(options), "Tail deve essere maggiore di zero.");
        var args = new List<string>
        {
            "logs",
            "--timestamps",
            "--tail",
            options.Tail.ToString(CultureInfo.InvariantCulture)
        };

        if (options.SinceUtc is { } since)
        {
            args.Add("--since");
            args.Add(since.ToUniversalTime().ToString(
                "yyyy-MM-ddTHH:mm:ss.fffffffZ",
                CultureInfo.InvariantCulture));
        }

        if (options.UntilUtc is { } until)
        {
            args.Add("--until");
            args.Add(until.ToUniversalTime().ToString(
                "yyyy-MM-ddTHH:mm:ss.fffffffZ",
                CultureInfo.InvariantCulture));
        }

        if (options.Follow)
            args.Add("--follow");

        args.Add(resource.ResourceId);

        var channel = Channel.CreateUnbounded<(string Stream, string Line)>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

        using var process = new System.Diagnostics.Process
        {
            StartInfo = DockerCli.CreateStartInfo(args),
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                channel.Writer.TryWrite(("stdout", e.Data));
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                channel.Writer.TryWrite(("stderr", e.Data));
        };

        process.Exited += (_, _) => channel.Writer.TryComplete();

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Completa il canale anche se il processo termina prima
            // che l'evento Exited venga gestito.
            _ = Task.Run(async () =>
            {
                try
                {
                    await process.WaitForExitAsync();
                    channel.Writer.TryComplete();
                }
                catch (Exception exception)
                {
                    channel.Writer.TryComplete(exception);
                }
            }, CancellationToken.None);

            await foreach (var item in channel.Reader.ReadAllAsync(cancellationToken))
            {
                var separator = item.Line.IndexOf(' ');
                if (separator > 0 && DateTime.TryParse(
                    item.Line[..separator], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var timestamp))
                {
                    yield return new RuntimeLogEntryDto(timestamp, item.Stream, item.Line[(separator + 1)..]);
                }
                else
                {
                    yield return new RuntimeLogEntryDto(DateTime.UtcNow, item.Stream, item.Line);
                }
            }

            await process.WaitForExitAsync(cancellationToken);
            if (process.ExitCode != 0) throw new InvalidOperationException($"Lettura log Docker fallita con exit code {process.ExitCode}.");
        }
        finally
        {
            if (!process.HasExited)
            {
                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch (InvalidOperationException)
                {
                    // Processo già terminato.
                }
            }
        }
    }
}
