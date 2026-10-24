using Athena.Cli.Application.Contracts.Systems;

namespace Athena.Cli.Application.Interfaces.Systems;

public interface ISystemsDiagnostic
{
    Task<RuntimeResourceStatusResponse> GetRuntimeStatusAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default);
    Task<RuntimeHealthCheckResponse> CheckRuntimeHealthAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RuntimeLogEntryResponse>> ReadRuntimeLogsAsync(Guid runtimeInstanceId, int tail = 100, DateTime? sinceUtc = null, DateTime? untilUtc = null, CancellationToken cancellationToken = default);
}