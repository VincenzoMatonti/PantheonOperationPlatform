using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;

namespace Athena.Cli.Application.Services.Systems;

public class SystemDiagnosticService(ISystemsDiagnostic systemsDiagnostic)
{
    public Task<RuntimeResourceStatusResponse> GetRuntimeStatusAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        systemsDiagnostic.GetRuntimeStatusAsync(runtimeInstanceId, cancellationToken);

    public Task<RuntimeHealthCheckResponse> CheckRuntimeHealthAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        systemsDiagnostic.CheckRuntimeHealthAsync(runtimeInstanceId, cancellationToken);

    public Task<IReadOnlyList<RuntimeLogEntryResponse>> ReadRuntimeLogsAsync(
        Guid runtimeInstanceId, int tail = 100, DateTime? sinceUtc = null, DateTime? untilUtc = null, CancellationToken cancellationToken = default) =>
        systemsDiagnostic.ReadRuntimeLogsAsync(runtimeInstanceId, tail, sinceUtc, untilUtc, cancellationToken);
}
