
using Athena.Application.SystemRegistry.Queries;
using Athena.Api.SystemRegistry.Requests;
using Athena.Application.SystemRegistry.Enums;

namespace Athena.Api.SystemRegistry.Mappings;

public static class SystemRegistryQueryRequestMapper
{
    public static GetSystemByIdQuery ToByIdQuery(Guid id) => new() { SystemId = id };
    public static GetSystemByNameQuery ToQuery(GetSystemByNameRequest request) => new() { Name = request.Name };
    public static GetSystemsByRegistrationStatusQuery ToQuery(SystemRegistrationStatusFilter status) => new() { Status = status };
    public static GetSystemsByAvailabilityStatusQuery ToQuery(SystemAvailabilityStatusFilter status) => new() { Status = status };
    public static GetSystemsByCapabilityQuery ToQuery(GetSystemsByCapabilityRequest request) => new() { CapabilityName = request.CapabilityName };
    public static GetSystemEndpointsQuery ToEndpointsQuery(Guid id) => new() { SystemId = id };
    public static GetSystemCapabilitiesQuery ToCapabilitiesQuery(Guid id) => new() { SystemId = id };
    public static GetSystemsByDeletionStatusQuery ToQuery(GetSystemsByDeletionStatusRequest request) => new() { IsDeleted = request.IsDeleted };
}
