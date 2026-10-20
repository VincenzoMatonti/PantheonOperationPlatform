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
[Route("api/systems/management")]
public class SystemLifecycleOperationCommandController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpPost("{systemId:guid}/lifecycle/operations")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationCommandResponse>>> Create(
        Guid systemId, [FromBody] CreateSystemLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCreateOperationCommand(systemId, request);
        var result = await handler.CreateSystemLifecycleOperationAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationCommandResponse>>> Start(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToStartOperationCommand(operationId);
        var result = await handler.StartSystemLifecycleOperationAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationCommandResponse>>> Complete(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCompleteOperationCommand(operationId);
        var result = await handler.CompleteSystemLifecycleOperationAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/fail")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationCommandResponse>>> Fail(
        Guid operationId, [FromBody] FailSystemLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToFailOperationCommand(operationId, request);
        var result = await handler.FailSystemLifecycleOperationAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationCommandResponse>>> Cancel(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToCancelOperationCommand(operationId);
        var result = await handler.CancelSystemLifecycleOperationAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("lifecycle-operations/{operationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationCommandResponse>>> Delete(Guid operationId, CancellationToken cancellationToken)
    {
        var command = SystemManagementRequestCommandMapper.ToDeleteOperationCommand(operationId);
        var result = await handler.DeleteSystemLifecycleOperationAsync(command, cancellationToken);
        var response = SystemManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
