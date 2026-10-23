using Athena.Application.Common.Dtos;

namespace Athena.Application.Common.Interfaces.Runtime;

/// <summary>
/// Reads logs produced by an external runtime resource.
/// </summary>
public interface IRuntimeLogReader
{
    string ProviderName { get; }

    IAsyncEnumerable<RuntimeLogEntryDto> ReadLogsAsync(RuntimeResourceReferenceDto resource, RuntimeLogOptionsDto options, CancellationToken cancellationToken = default);
}