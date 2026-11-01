using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Application.SystemManagement.Contracts;
using Athena.Application.SystemManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/system-instances")]
public class ApiSystemInstanceProcessController(SystemInstanceProcessUseCaseHandler service) : ControllerBase
{
    private readonly SystemInstanceProcessUseCaseHandler _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemInstanceProcessDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemInstanceProcessDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.GetAllAsync(cancellationToken)));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceProcessDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemInstanceProcessDto>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> Create(
        [FromBody] SystemInstanceExecutionCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.SystemInstanceId }, ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> Register(
        [FromBody] SystemInstanceExecutionCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RegisterAsync(request, cancellationToken);
        return Ok(ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> Start([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.StartAsync(id, cancellationToken)));
    }

    [HttpPost("{id:guid}/stop")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> Stop([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.StopAsync(id, cancellationToken)));
    }

    [HttpPost("{id:guid}/restart")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> Restart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.RestartAsync(id, cancellationToken)));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> Remove([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.RemoveAsync(id, cancellationToken)));
    }

    [HttpGet("{id:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatus>>> GetStatus([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.GetStatusAsync(id, cancellationToken)));
    }

    [HttpGet("{id:guid}/health")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionHealth>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionHealth>>> GetHealth([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.GetHealthAsync(id, cancellationToken)));
    }

    [HttpGet("{id:guid}/logs")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemInstanceExecutionLogEntry>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemInstanceExecutionLogEntry>>>> GetLogs(
        [FromRoute] Guid id,
        [FromQuery] int tail = 100,
        CancellationToken cancellationToken = default)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.GetLogsAsync(id, tail, cancellationToken)));
    }

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemInstanceOperationDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemInstanceOperationDto>>>> GetOperations(
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(ApiResponseBuilder.BuildSuccess(await _service.GetOperationsAsync(id, cancellationToken)));
    }
}
