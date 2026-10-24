using Athena.Api.SystemManagement.Requests;
using Athena.Application.SystemManagement.Commands;

namespace Athena.Api.SystemManagement.Mappings;

public static class SystemDiagnosticRequestCommandMapper
{
    public static GetRuntimeStatusCommand ToGetRuntimeStatusCommand(Guid runtimeInstanceId)
    {
        return new GetRuntimeStatusCommand(runtimeInstanceId);
    }

    public static CheckRuntimeHealthCommand ToCheckRuntimeHealthCommand(Guid runtimeInstanceId)
    {
        return new CheckRuntimeHealthCommand(runtimeInstanceId);
    }

    public static ReadRuntimeLogsCommand ToReadRuntimeLogsCommand(Guid runtimeInstanceId, ReadRuntimeLogsRequest request)
    {
        return new ReadRuntimeLogsCommand(runtimeInstanceId, request.Tail, request.SinceUtc, request.UntilUtc);
    }
}