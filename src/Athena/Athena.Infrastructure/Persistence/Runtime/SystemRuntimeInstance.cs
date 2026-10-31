using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Infrastructure.Persistence.Runtime;

/// <summary>
/// Technical persistence model linking a Pantheon system to a runtime resource.
/// This is not a domain entity.
/// </summary>
public sealed class SystemRuntimeInstance
{
    public Guid Id { get; set; }

    public SystemId SystemId { get; set; } = null!;

    public string RuntimeProvider { get; set; } = string.Empty;

    public string RuntimeResourceId { get; set; } = string.Empty;

    public string RuntimeResourceName { get; set; } = string.Empty;

    public string Environment { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}