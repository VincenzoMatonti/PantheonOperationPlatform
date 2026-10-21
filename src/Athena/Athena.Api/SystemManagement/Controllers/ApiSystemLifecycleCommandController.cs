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
[Route("api/systems/management")]
public class SystemLifecycleCommandController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpPost("{systemId:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> Create(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCreateLifecycleCommand(systemId);
        var result = await handler.CreateSystemLifecycleAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{systemId:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> SetDesiredState(
        Guid systemId, [FromBody] SetSystemDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToSetDesiredStateCommand(systemId, request);
        var result = await handler.SetSystemDesiredStateAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{systemId:guid}/lifecycle/starting")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> MarkStarting(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkStartingCommand(systemId);
        var result = await handler.MarkSystemStartingAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{systemId:guid}/lifecycle/running")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> MarkRunning(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkRunningCommand(systemId);
        var result = await handler.MarkSystemRunningAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{systemId:guid}/lifecycle/stopping")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> MarkStopping(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkStoppingCommand(systemId);
        var result = await handler.MarkSystemStoppingAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{systemId:guid}/lifecycle/stopped")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> MarkStopped(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkStoppedCommand(systemId);
        var result = await handler.MarkSystemStoppedAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{systemId:guid}/lifecycle/failed")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> MarkFailed(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkFailedCommand(systemId);
        var result = await handler.MarkSystemFailedAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{systemId:guid}/lifecycle/unknown")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> MarkUnknown(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkUnknownCommand(systemId);
        var result = await handler.MarkSystemUnknownAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{systemId:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleCommandResponse>>> Delete(
        Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToDeleteLifecycleCommand(systemId);
        var result = await handler.DeleteSystemLifecycleAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
