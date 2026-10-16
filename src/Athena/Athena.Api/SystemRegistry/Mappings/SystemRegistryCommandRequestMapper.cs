
using Athena.Application.SystemRegistry.Commands;
using Athena.Api.SystemRegistry.Requests;

namespace Athena.Api.SystemRegistry.Mappings;

public static class SystemRegistryCommandRequestMapper
{
    public static RegisterSystemCommand ToCommand(RegisterSystemRequest request) => new() { Name = request.Name };
    public static RenameSystemCommand ToCommand(Guid id, RenameSystemRequest request) => new() { SystemId = id, NewName = request.NewName };
    public static UnregisterSystemCommand ToUnregisterCommand(Guid id) => new() { SystemId = id };
    public static EnableSystemCommand ToEnableCommand(Guid id) => new() { SystemId = id };
    public static DisableSystemCommand ToDisableCommand(Guid id) => new() { SystemId = id };
    public static MarkSystemAvailableCommand ToMarkAvailableCommand(Guid id) => new() { SystemId = id };
    public static MarkSystemUnavailableCommand ToMarkUnavailableCommand(Guid id) => new() { SystemId = id };
    public static MarkSystemAvailabilityUnknownCommand ToMarkUnknownCommand(Guid id) => new() { SystemId = id };

    public static AddSystemEndpointCommand ToCommand(Guid id, AddSystemEndpointRequest request) => new()
    {
        SystemId = id,
        Name = request.Name,
        BaseAddress = request.BaseAddress
    };

    public static RemoveSystemEndpointCommand ToCommand(Guid id, RemoveSystemEndpointRequest request) => new()
    {
        SystemId = id,
        EndpointName = request.EndpointName
    };

    public static AddSystemCapabilityCommand ToCommand(Guid id, AddSystemCapabilityRequest request) => new()
    {
        SystemId = id,
        Name = request.Name,
        Description = request.Description
    };

    public static RemoveSystemCapabilityCommand ToCommand(Guid id, RemoveSystemCapabilityRequest request) => new()
    {
        SystemId = id,
        CapabilityName = request.CapabilityName
    };

    public static DeleteSystemCommand ToDeleteCommand(Guid id) => new() { SystemId = id };
}
