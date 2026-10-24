using Athena.Application.Common.Dtos;
using Athena.Application.Common.Services;
using Athena.Application.SystemManagement.Commands;

namespace Athena.Application.SystemManagement.UseCases;

/// <summary>
/// Coordinates read-only diagnostics for registered runtime instances.
/// Does not modify system or orchestration lifecycle state.
/// </summary>
public class SystemDiagnosticUseCaseHandler(ApplicationExceptionService exceptionService, RuntimeApplicationService runtimeApplicationService)
{
    private const int DefaultTail = 100;
    private const int MaxTail = 1000;

    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly RuntimeApplicationService _runtimeApplicationService = runtimeApplicationService;

    public Task<RuntimeResourceStatusDto> GetRuntimeStatusAsync(GetRuntimeStatusCommand command, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(() => _runtimeApplicationService.GetStatusAsync(command.RuntimeInstanceId, cancellationToken));
    }

    public Task<RuntimeHealthCheckDto> CheckRuntimeHealthAsync(CheckRuntimeHealthCommand command, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(() => _runtimeApplicationService.CheckHealthAsync(command.RuntimeInstanceId, cancellationToken));
    }

    public Task<IReadOnlyList<RuntimeLogEntryDto>> ReadRuntimeLogsAsync(ReadRuntimeLogsCommand command, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var options = NormalizeLogOptions(new RuntimeLogOptionsDto(Tail: command.Tail, SinceUtc: command.SinceUtc, UntilUtc: command.UntilUtc, Follow: false));
            var entries = new List<RuntimeLogEntryDto>(options.Tail);
            await foreach (var entry in _runtimeApplicationService.ReadLogsAsync(command.RuntimeInstanceId, options, cancellationToken).WithCancellation(cancellationToken))
            {
                entries.Add(entry);
            }

            return (IReadOnlyList<RuntimeLogEntryDto>)entries;
        });
    }

    private static RuntimeLogOptionsDto NormalizeLogOptions(RuntimeLogOptionsDto? options)
    {
        options ??= new RuntimeLogOptionsDto(Tail: DefaultTail);
        if (options.Tail < 1 || options.Tail > MaxTail) throw new ArgumentOutOfRangeException(nameof(options), $"Tail deve essere compreso tra 1 e {MaxTail}.");
        if (options.Follow) throw new ArgumentException("Follow=true non è supportato dalla lettura dei log con risposta finita.", nameof(options));
        if (options.SinceUtc.HasValue && options.UntilUtc.HasValue && options.SinceUtc.Value > options.UntilUtc.Value)
            throw new ArgumentException("SinceUtc non può essere successivo a UntilUtc.", nameof(options));

        return options;
    }
}