using Athena.Api.OrchestrationRegistry.Mappings;
using Athena.Api.OrchestrationRegistry.Requests;
using Athena.Api.OrchestrationRegistry.Responses;
using Athena.Application.OrchestrationRegistry.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.OrchestrationRegistry.Controllers;

[ApiController]
[Route("api/orchestrations")]
public class OrchestrationRegistryCommandController(OrchestrationRegistryUseCaseHandler handler) : ControllerBase
{
    private readonly OrchestrationRegistryUseCaseHandler _handler = handler;

    // =====================================================================
    // COMMANDS
    [HttpPost]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrchestrationResponse>> Create([FromBody] CreateOrchestrationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToCommand(request);
        var result = await _handler.CreateAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Created($"/api/orchestrations/{response.OrchestrationId}", response);
    }

    [HttpPost("{id:guid}/register")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> Register(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToRegisterCommand(id);
        var result = await _handler.RegisterAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/unregister")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> Unregister(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToUnregisterCommand(id);
        var result = await _handler.UnregisterAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/enable")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> Enable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToEnableCommand(id);
        var result = await _handler.EnableAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/disable")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> Disable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToDisableCommand(id);
        var result = await _handler.DisableAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPatch("{id:guid}/name")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> Rename(Guid id, [FromBody] RenameOrchestrationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToRenameCommand(id, request);
        var result = await _handler.RenameAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/availability/available")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> MarkAvailable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToMarkAvailableCommand(id);
        var result = await _handler.MarkAvailableAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/availability/unavailable")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> MarkUnavailable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToMarkUnavailableCommand(id);
        var result = await _handler.MarkUnavailableAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpPost("{id:guid}/availability/unknown")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> MarkAvailabilityUnknown(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToMarkAvailabilityUnknownCommand(id);
        var result = await _handler.MarkAvailabilityUnknownAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToDeleteCommand(id);
        var result = await _handler.DeleteAsync(command, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }
}