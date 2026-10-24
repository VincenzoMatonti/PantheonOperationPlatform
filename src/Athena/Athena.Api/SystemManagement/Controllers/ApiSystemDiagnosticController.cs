using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.SystemManagement.Mappings;
using Athena.Api.SystemManagement.Requests;
using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/systems/diagnostics")]
public class ApiSystemDiagnosticController(SystemDiagnosticUseCaseHandler diagnosticHandler) : ControllerBase
{
    private readonly SystemDiagnosticUseCaseHandler _diagnosticHandler = diagnosticHandler;

    // =====================================================================
    // RUNTIME STATUS
    [HttpGet("runtime-instances/{runtimeInstanceId:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<RuntimeResourceStatusResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RuntimeResourceStatusResponse>>> GetRuntimeStatus(Guid runtimeInstanceId, CancellationToken cancellationToken)
    {
        var command = SystemDiagnosticRequestCommandMapper.ToGetRuntimeStatusCommand(runtimeInstanceId);
        var result = await _diagnosticHandler.GetRuntimeStatusAsync(command, cancellationToken);
        var response = SystemDiagnosticResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    // =====================================================================
    // RUNTIME HEALTH CHECK
    [HttpGet("runtime-instances/{runtimeInstanceId:guid}/health")]
    [ProducesResponseType(typeof(ApiResponse<RuntimeHealthCheckResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RuntimeHealthCheckResponse>>> CheckRuntimeHealth(Guid runtimeInstanceId, CancellationToken cancellationToken)
    {
        var command = SystemDiagnosticRequestCommandMapper.ToCheckRuntimeHealthCommand(runtimeInstanceId);
        var result = await _diagnosticHandler.CheckRuntimeHealthAsync(command, cancellationToken);
        var response = SystemDiagnosticResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    // =====================================================================
    // RUNTIME LOGS
    [HttpGet("runtime-instances/{runtimeInstanceId:guid}/logs")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RuntimeLogEntryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RuntimeLogEntryResponse>>>> ReadRuntimeLogs(
        Guid runtimeInstanceId, [FromQuery] ReadRuntimeLogsRequest request, CancellationToken cancellationToken)
    {
        var command = SystemDiagnosticRequestCommandMapper.ToReadRuntimeLogsCommand(runtimeInstanceId, request);
        var result = await _diagnosticHandler.ReadRuntimeLogsAsync(command, cancellationToken);
        var response = SystemDiagnosticResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}