using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
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
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Create(
        [FromBody] RegisterSystemRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(request);
        var result = await _handler.CreateSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Register(
        [FromBody] RegisterSystemRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(request);
        var result = await _handler.RegisterSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/unregister")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Unregister(
        Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToUnregisterCommand(id);
        var result = await _handler.UnregisterSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{id:guid}/name")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Rename(
        Guid id, [FromBody] RenameSystemRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.RenameSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/enable")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Enable(
        Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToEnableCommand(id);
        var result = await _handler.EnableSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/disable")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Disable(
        Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToDisableCommand(id);
        var result = await _handler.DisableSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/availability/available")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> MarkAvailable(
        Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToMarkAvailableCommand(id);
        var result = await _handler.MarkSystemAvailableAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/availability/unavailable")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> MarkUnavailable(
        Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToMarkUnavailableCommand(id);
        var result = await _handler.MarkSystemUnavailableAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/availability/unknown")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> MarkAvailabilityUnknown(
        Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToMarkUnknownCommand(id);
        var result = await _handler.MarkSystemAvailabilityUnknownAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/endpoints")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> AddEndpoint(
        Guid id, [FromBody] AddSystemEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.AddSystemEndpointAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}/endpoints")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> RemoveEndpoint(
        Guid id, [FromBody] RemoveSystemEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.RemoveSystemEndpointAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/capabilities")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> AddCapability(
        Guid id, [FromBody] AddSystemCapabilityRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.AddSystemCapabilityAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}/capabilities")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> RemoveCapability(
        Guid id, [FromBody] RemoveSystemCapabilityRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToCommand(id, request);
        var result = await _handler.RemoveSystemCapabilityAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = SystemRegistryCommandRequestMapper.ToDeleteCommand(id);
        var result = await _handler.DeleteSystemAsync(command, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}