using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
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

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Create(
        [FromBody] CreateOrchestrationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToCommand(request);
        var result = await _handler.CreateAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/register")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Register(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToRegisterCommand(id);
        var result = await _handler.RegisterAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/unregister")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Unregister(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToUnregisterCommand(id);
        var result = await _handler.UnregisterAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/enable")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Enable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToEnableCommand(id);
        var result = await _handler.EnableAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/disable")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Disable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToDisableCommand(id);
        var result = await _handler.DisableAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{id:guid}/name")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Rename(
        Guid id, [FromBody] RenameOrchestrationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToRenameCommand(id, request);
        var result = await _handler.RenameAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/availability/available")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> MarkAvailable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToMarkAvailableCommand(id);
        var result = await _handler.MarkAvailableAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/availability/unavailable")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> MarkUnavailable(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToMarkUnavailableCommand(id);
        var result = await _handler.MarkUnavailableAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/availability/unknown")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> MarkAvailabilityUnknown(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToMarkAvailabilityUnknownCommand(id);
        var result = await _handler.MarkAvailabilityUnknownAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationRegistryCommandRequestMapper.ToDeleteCommand(id);
        var result = await _handler.DeleteAsync(command, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}