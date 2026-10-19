using Athena.Api.SystemManagement.Mappings;
using Athena.Api.SystemManagement.Requests;
using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/systems/mangement")]
public class SystemLifecycleOperationCommandController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpPost("{systemId:guid}/lifecycle/operations")]
    public async Task<ActionResult<SystemLifecycleOperationCommandResponse>> Create(
        Guid systemId,
        [FromBody] CreateSystemLifecycleOperationRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCreateOperationCommand(systemId, request);
        var result = await handler.CreateSystemLifecycleOperationAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/start")]
    public async Task<ActionResult<SystemLifecycleOperationCommandResponse>> Start(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToStartOperationCommand(operationId);
        var result = await handler.StartSystemLifecycleOperationAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/complete")]
    public async Task<ActionResult<SystemLifecycleOperationCommandResponse>> Complete(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCompleteOperationCommand(operationId);
        var result = await handler.CompleteSystemLifecycleOperationAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/fail")]
    public async Task<ActionResult<SystemLifecycleOperationCommandResponse>> Fail(
        Guid operationId,
        [FromBody] FailSystemLifecycleOperationRequest request,
        CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToFailOperationCommand(operationId, request);
        var result = await handler.FailSystemLifecycleOperationAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/cancel")]
    public async Task<ActionResult<SystemLifecycleOperationCommandResponse>> Cancel(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCancelOperationCommand(operationId);
        var result = await handler.CancelSystemLifecycleOperationAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpDelete("lifecycle-operations/{operationId:guid}")]
    public async Task<ActionResult<SystemLifecycleOperationCommandResponse>> Delete(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToDeleteOperationCommand(operationId);
        var result = await handler.DeleteSystemLifecycleOperationAsync(command, cancellationToken);
        return Ok(SystemManagementResponseCommandMapper.ToResponse(result));
    }
}