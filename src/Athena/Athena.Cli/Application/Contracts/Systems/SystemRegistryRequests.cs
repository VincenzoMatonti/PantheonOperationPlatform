namespace Athena.Cli.Application.Contracts.Systems;

public sealed record RegisterSystemRequest(string Name);
public sealed record RenameSystemRequest(string NewName);
public sealed record AddSystemEndpointRequest(string Name, string BaseAddress);
public sealed record RemoveSystemEndpointRequest(string EndpointName);
public sealed record AddSystemCapabilityRequest(string Name, string Description);
public sealed record RemoveSystemCapabilityRequest(string CapabilityName);
