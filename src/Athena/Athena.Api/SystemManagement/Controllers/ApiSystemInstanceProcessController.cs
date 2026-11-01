using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.SystemManagement.Mappings;
using Athena.Api.SystemManagement.Requests;
using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.Services;
using Athena.Domain.System.Management.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/system-instances/management")]
public sealed class ApiSystemInstanceProcessController(SystemInstanceProcessUseCaseHandler handler) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemInstanceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemInstanceResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToGetAllQuery();
        var result = await handler.GetAllAsync(query, cancellationToken);
        return Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToResponses(result)));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemInstanceResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToGetByIdQuery(id);
        var result = await handler.GetByIdAsync(query, cancellationToken);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToResponse(result)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Create(
        [FromBody] CreateSystemInstanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemInstanceProcessRequestMapper.ToCreateCommand(request);
        var operation = await handler.CreateAsync(command, cancellationToken);
        var response = SystemInstanceProcessResponseMapper.ToResponse(operation);
        return AcceptedAtAction(nameof(GetOperation),
            new { id = operation.SystemInstanceId, operationId = operation.Id },
            ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Register(
        [FromBody] RegisterSystemInstanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemInstanceProcessRequestMapper.ToRegisterCommand(request);
        var operation = await handler.RegisterAsync(command, cancellationToken);
        var response = SystemInstanceProcessResponseMapper.ToResponse(operation);
        return AcceptedAtAction(nameof(GetOperation),
            new { id = operation.SystemInstanceId, operationId = operation.Id },
            ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Start(Guid id, CancellationToken cancellationToken) =>
        EnqueueOperation(id, SystemInstanceOperationType.Start, cancellationToken);

    [HttpPost("{id:guid}/stop")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Stop(Guid id, CancellationToken cancellationToken) =>
        EnqueueOperation(id, SystemInstanceOperationType.Stop, cancellationToken);

    [HttpPost("{id:guid}/restart")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Restart(Guid id, CancellationToken cancellationToken) =>
        EnqueueOperation(id, SystemInstanceOperationType.Restart, cancellationToken);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Remove(Guid id, CancellationToken cancellationToken) =>
        EnqueueOperation(id, SystemInstanceOperationType.Remove, cancellationToken);

    [HttpPost("{id:guid}/unregister")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Unregister(Guid id, CancellationToken cancellationToken) =>
        EnqueueOperation(id, SystemInstanceOperationType.Unregister, cancellationToken);

    [HttpPost("{id:guid}/configure")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> Configure(
        Guid id,
        [FromBody] ConfigureSystemInstanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemInstanceProcessRequestMapper.ToConfigureCommand(id, request);
        var operation = await handler.ConfigureAsync(command, cancellationToken);
        var response = SystemInstanceProcessResponseMapper.ToResponse(operation);
        return AcceptedAtAction(nameof(GetOperation),
            new { id, operationId = operation.Id },
            ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{id:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionStatusResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionStatusResponse>>> GetStatus(Guid id, CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToStatusQuery(id);
        var result = await handler.GetStatusAsync(query, cancellationToken);
        return Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToResponse(result)));
    }

    [HttpGet("{id:guid}/health")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceExecutionHealthResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInstanceExecutionHealthResponse>>> GetHealth(Guid id, CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToHealthQuery(id);
        var result = await handler.GetHealthAsync(query, cancellationToken);
        return Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToResponse(result)));
    }

    [HttpGet("{id:guid}/logs")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemInstanceExecutionLogResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemInstanceExecutionLogResponse>>>> GetLogs(
        Guid id,
        [FromQuery] GetSystemInstanceLogsRequest request,
        CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToLogsQuery(id, request);
        var result = await handler.GetLogsAsync(query, cancellationToken);
        return Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToLogResponses(result)));
    }

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemInstanceOperationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemInstanceOperationResponse>>>> GetOperations(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToOperationsQuery(id);
        var result = await handler.GetOperationsAsync(query, cancellationToken);
        return Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToOperationResponses(result)));
    }

    [HttpGet("{id:guid}/operations/{operationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> GetOperation(
        Guid id,
        Guid operationId,
        CancellationToken cancellationToken)
    {
        var query = SystemInstanceProcessRequestMapper.ToOperationQuery(id, operationId);
        var result = await handler.GetOperationAsync(query, cancellationToken);
        return result is null
            ? NotFound()
            : Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/operations/{operationId:guid}/resolve-unknown")]
    [ProducesResponseType(typeof(ApiResponse<SystemInstanceOperationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> ResolveUnknownOperation(
        Guid id,
        Guid operationId,
        [FromBody] ResolveSystemInstanceOperationRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemInstanceProcessRequestMapper.ToResolveOutcomeUnknownCommand(id, operationId, request);
        var result = await handler.ResolveOutcomeUnknownOperationAsync(command, cancellationToken);
        return result is null
            ? NotFound()
            : Ok(ApiResponseBuilder.BuildSuccess(SystemInstanceProcessResponseMapper.ToResponse(result)));
    }

    private async Task<ActionResult<ApiResponse<SystemInstanceOperationResponse>>> EnqueueOperation(
        Guid id,
        SystemInstanceOperationType type,
        CancellationToken cancellationToken)
    {
        var command = SystemInstanceProcessRequestMapper.ToOperationCommand(id, type);
        var operation = await handler.EnqueueOperationAsync(command, cancellationToken);
        var response = SystemInstanceProcessResponseMapper.ToResponse(operation);
        return AcceptedAtAction(nameof(GetOperation),
            new { id, operationId = operation.Id },
            ApiResponseBuilder.BuildSuccess(response));
    }
}
