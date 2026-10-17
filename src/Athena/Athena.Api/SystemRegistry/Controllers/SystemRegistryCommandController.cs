using Athena.Api.SystemRegistry.Mappings;
using Athena.Api.SystemRegistry.Requests;
using Athena.Api.SystemRegistry.Responses;
using Athena.Application.SystemRegistry.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemRegistry.Controllers;

[ApiController]
[Route("api/systems")]
public class SystemRegistryCommandController(SystemRegistryUseCaseHandler handler) : ControllerBase
{
    private readonly SystemRegistryUseCaseHandler _handler = handler;

    [HttpPost]
    [ProducesResponseType<SystemResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<SystemResponse>> Create([FromBody] RegisterSystemRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(request);
        var result = await _handler.CreateSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("register")]
    [ProducesResponseType<SystemResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<SystemResponse>> Register([FromBody] RegisterSystemRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(request);
        var result = await _handler.RegisterSystemAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/unregister")]
    public async Task<ActionResult<SystemResponse>> Unregister(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToUnregisterCommand(id);
        var result = await _handler.UnregisterSystemAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPatch("{id:guid}/name")]
    public async Task<ActionResult<SystemResponse>> Rename(Guid id, [FromBody] RenameSystemRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.RenameSystemAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/enable")]
    public async Task<ActionResult<SystemResponse>> Enable(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToEnableCommand(id);
        var result = await _handler.EnableSystemAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/disable")]
    public async Task<ActionResult<SystemResponse>> Disable(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToDisableCommand(id);
        var result = await _handler.DisableSystemAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/availability/available")]
    public async Task<ActionResult<SystemResponse>> MarkAvailable(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToMarkAvailableCommand(id);
        var result = await _handler.MarkSystemAvailableAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/availability/unavailable")]
    public async Task<ActionResult<SystemResponse>> MarkUnavailable(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToMarkUnavailableCommand(id);
        var result = await _handler.MarkSystemUnavailableAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/availability/unknown")]
    public async Task<ActionResult<SystemResponse>> MarkAvailabilityUnknown(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToMarkUnknownCommand(id);
        var result = await _handler.MarkSystemAvailabilityUnknownAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/endpoints")]
    public async Task<ActionResult<SystemResponse>> AddEndpoint(Guid id, [FromBody] AddSystemEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.AddSystemEndpointAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpDelete("{id:guid}/endpoints")]
    public async Task<ActionResult<SystemResponse>> RemoveEndpoint(Guid id, [FromBody] RemoveSystemEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.RemoveSystemEndpointAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/capabilities")]
    public async Task<ActionResult<SystemResponse>> AddCapability(Guid id, [FromBody] AddSystemCapabilityRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.AddSystemCapabilityAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpDelete("{id:guid}/capabilities")]
    public async Task<ActionResult<SystemResponse>> RemoveCapability(Guid id, [FromBody] RemoveSystemCapabilityRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.RemoveSystemCapabilityAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<SystemResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToDeleteCommand(id);
        var result = await _handler.DeleteSystemAsync(command, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }
}
