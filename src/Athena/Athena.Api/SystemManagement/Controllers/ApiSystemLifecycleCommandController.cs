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
    public async Task<ActionResult<SystemLifecycleCommandResponse>> Create(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCreateLifecycleCommand(systemId);
        var result = await handler.CreateSystemLifecycleAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPatch("{systemId:guid}/lifecycle/desired-state")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> SetDesiredState(
        Guid systemId,
        [FromBody] SetSystemDesiredStateRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToSetDesiredStateCommand(systemId, request);
        var result = await handler.SetSystemDesiredStateAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{systemId:guid}/lifecycle/starting")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> MarkStarting(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkStartingCommand(systemId);
        var result = await handler.MarkSystemStartingAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{systemId:guid}/lifecycle/running")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> MarkRunning(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkRunningCommand(systemId);
        var result = await handler.MarkSystemRunningAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{systemId:guid}/lifecycle/stopping")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> MarkStopping(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkStoppingCommand(systemId);
        var result = await handler.MarkSystemStoppingAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{systemId:guid}/lifecycle/stopped")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> MarkStopped(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkStoppedCommand(systemId);
        var result = await handler.MarkSystemStoppedAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{systemId:guid}/lifecycle/failed")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> MarkFailed(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkFailedCommand(systemId);
        var result = await handler.MarkSystemFailedAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{systemId:guid}/lifecycle/unknown")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> MarkUnknown(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToMarkUnknownCommand(systemId);
        var result = await handler.MarkSystemUnknownAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpDelete("{systemId:guid}/lifecycle")]
    public async Task<ActionResult<SystemLifecycleCommandResponse>> Delete(Guid systemId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToDeleteLifecycleCommand(systemId);
        var result = await handler.DeleteSystemLifecycleAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }
}