using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Application.SystemManagement.Contracts;
using Athena.Application.SystemManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/system-instances/management")]
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
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationDto>), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationDto>>> Start([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var operation = await _service.EnqueueOperationAsync(id, Athena.Domain.System.Management.Enums.SystemInstanceOperationType.Start, cancellationToken);
        return AcceptedAtAction(nameof(GetOperation), new { id, operationId = operation.Id }, ApiResponseBuilder.BuildSuccess(operation));
    }

    [HttpPost("{id:guid}/stop")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationDto>), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationDto>>> Stop([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var operation = await _service.EnqueueOperationAsync(id, Athena.Domain.System.Management.Enums.SystemInstanceOperationType.Stop, cancellationToken);
        return AcceptedAtAction(nameof(GetOperation), new { id, operationId = operation.Id }, ApiResponseBuilder.BuildSuccess(operation));
    }

    [HttpPost("{id:guid}/restart")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationDto>), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationDto>>> Restart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var operation = await _service.EnqueueOperationAsync(id, Athena.Domain.System.Management.Enums.SystemInstanceOperationType.Restart, cancellationToken);
        return AcceptedAtAction(nameof(GetOperation), new { id, operationId = operation.Id }, ApiResponseBuilder.BuildSuccess(operation));
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

    [HttpGet("{id:guid}/operations/{operationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationDto>>> GetOperation(
        [FromRoute] Guid id,
        [FromRoute] Guid operationId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetOperationAsync(id, operationId, cancellationToken);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost("{id:guid}/operations/{operationId:guid}/resolve-unknown")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationDto>>> ResolveUnknownOperation(
        [FromRoute] Guid id,
        [FromRoute] Guid operationId,
        [FromBody] ResolveSystemInstanceOperationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ResolveOutcomeUnknownOperationAsync(id, operationId, request.Reason, cancellationToken);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(result));
    }
}
