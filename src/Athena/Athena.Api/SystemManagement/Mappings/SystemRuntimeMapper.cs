using Athena.Api.SystemManagement.Requests;
using Athena.Api.SystemManagement.Responses;
using Athena.Application.Common.Dtos;
using Athena.Application.SystemManagement.Dtos;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemRuntimeRequestMapper
{
    public static GetSystemRuntimeInstanceByIdQuery ToQuery(GetSystemRuntimeInstanceByIdRequest request) => new() { Id = request.Id };

    public static GetSystemRuntimeInstancesBySystemIdQuery ToQuery(GetSystemRuntimeInstancesBySystemIdRequest request) =>
        new() { SystemId = request.SystemId };

    public static GetSystemRuntimeInstancesBySystemIdAndEnvironmentQuery ToQuery(GetSystemRuntimeInstancesBySystemIdAndEnvironmentRequest request) =>
        new() { SystemId = request.SystemId, Environment = request.Environment };

    public static GetSystemRuntimeInstancesByRuntimeResourceNameQuery ToQuery(GetSystemRuntimeInstancesByRuntimeResourceNameRequest request) =>
        new() { RuntimeResourceName = request.RuntimeResourceName };

    public static RegisterSystemRuntimeInstanceCommand ToCommand(RegisterSystemRuntimeInstanceRequest request) =>
        new() { SystemId = request.SystemId, RuntimeProvider = request.RuntimeProvider, RuntimeResourceId = request.RuntimeResourceId, RuntimeResourceName = request.RuntimeResourceName, Environment = request.Environment };

    public static UnregisterSystemRuntimeInstanceCommand ToCommand(UnregisterSystemRuntimeInstanceRequest request) => new() { Id = request.Id };
}

public static class SystemRuntimeResponseMapper
{
    public static SystemRuntimeInstanceResponse ToResponse(SystemRuntimeInstanceDto dto) =>
        new(dto.Id, dto.SystemId, dto.RuntimeProvider, dto.RuntimeResourceId, dto.RuntimeResourceName, dto.Environment, dto.CreatedAtUtc, dto.UpdatedAtUtc);

    public static IReadOnlyList<SystemRuntimeInstanceResponse> ToResponse(IReadOnlyList<SystemRuntimeInstanceDto> dtos) => [.. dtos.Select(ToResponse)];
}
