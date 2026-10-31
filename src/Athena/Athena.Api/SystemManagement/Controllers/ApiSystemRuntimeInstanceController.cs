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
[Route("api/systems/runtime-instances")]
public class ApiSystemRuntimeInstanceController(SystemRuntimeUseCaseHandler runtimeHandler) : ControllerBase
{
    private readonly SystemRuntimeUseCaseHandler _runtimeHandler = runtimeHandler;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _runtimeHandler.GetAllAsync(cancellationToken);
        var response = SystemRuntimeResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemRuntimeInstanceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemRuntimeInstanceResponse>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSystemRuntimeInstanceByIdRequest { Id = id };
        var query = SystemRuntimeRequestMapper.ToQuery(request);
        var result = await _runtimeHandler.GetByIdAsync(query, cancellationToken);
        if (result is null) return NotFound();
        var response = SystemRuntimeResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-system/{systemId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>>> GetBySystemId([FromRoute] Guid systemId, CancellationToken cancellationToken)
    {
        var request = new GetSystemRuntimeInstancesBySystemIdRequest { SystemId = systemId };
        var query = SystemRuntimeRequestMapper.ToQuery(request);
        var result = await _runtimeHandler.GetBySystemIdAsync(query, cancellationToken);
        var response = SystemRuntimeResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-system/{systemId:guid}/environment/{environment}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>>> GetBySystemIdAndEnvironment(
            [FromRoute] Guid systemId, [FromRoute] string environment, CancellationToken cancellationToken)
    {
        var request = new GetSystemRuntimeInstancesBySystemIdAndEnvironmentRequest { SystemId = systemId, Environment = environment };
        var query = SystemRuntimeRequestMapper.ToQuery(request);
        var result = await _runtimeHandler.GetBySystemIdAndEnvironmentAsync(query, cancellationToken);
        var response = SystemRuntimeResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-resource-name/{runtimeResourceName}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemRuntimeInstanceResponse>>>> GetByRuntimeResourceName(
            [FromRoute] string runtimeResourceName, CancellationToken cancellationToken)
    {
        var request = new GetSystemRuntimeInstancesByRuntimeResourceNameRequest { RuntimeResourceName = runtimeResourceName };
        var query = SystemRuntimeRequestMapper.ToQuery(request);
        var result = await _runtimeHandler.GetByRuntimeResourceNameAsync(query, cancellationToken);
        var response = SystemRuntimeResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SystemRuntimeInstanceResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<SystemRuntimeInstanceResponse>>> Register(
            [FromBody] RegisterSystemRuntimeInstanceRequest request, CancellationToken cancellationToken)
    {
        var command = SystemRuntimeRequestMapper.ToCommand(request);
        var result = await _runtimeHandler.RegisterAsync(command, cancellationToken);
        var response = SystemRuntimeResponseMapper.ToResponse(result);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unregister([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new UnregisterSystemRuntimeInstanceRequest { Id = id };
        var command = SystemRuntimeRequestMapper.ToCommand(request);
        var removed = await _runtimeHandler.UnregisterAsync(command, cancellationToken);
        return removed ? NoContent() : NotFound();
    }
}
