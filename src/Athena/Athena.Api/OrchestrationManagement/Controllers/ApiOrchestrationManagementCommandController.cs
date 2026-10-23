using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.OrchestrationManagement.Mappings;
using Athena.Api.OrchestrationManagement.Requests;
using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.OrchestrationManagement.Controllers;

[ApiController]
[Route("api/orchestrations/management")]
public class OrchestrationManagementCommandController(
    OrchestrationLifecycleUseCaseHandler lifecycleHandler,
    OrchestrationLifecycleOperationUseCaseHandler operationHandler) : ControllerBase
{
    private readonly OrchestrationLifecycleUseCaseHandler _lifecycleHandler = lifecycleHandler;
    private readonly OrchestrationLifecycleOperationUseCaseHandler _operationHandler = operationHandler;

    // =====================================================================
    // ORCHESTRATION LIFECYCLE COMMANDS
    [HttpPost("{orchestrationId:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> CreateLifecycle(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var result = await _lifecycleHandler.CreateOrchestrationLifecycleAsync(orchestrationId, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{orchestrationId:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> SetDesiredState(
        Guid orchestrationId, [FromBody] SetOrchestrationDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToSetDesiredStateCommand(orchestrationId, request);
        var result = await _lifecycleHandler.SetOrchestrationDesiredStateAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/starting")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> MarkStarting(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkStartingCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationStartingAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/running")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> MarkRunning(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkRunningCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationRunningAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/stopping")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> MarkStopping(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkStoppingCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationStoppingAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/stopped")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> MarkStopped(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkStoppedCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationStoppedAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/failed")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> MarkFailed(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkFailedCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationFailedAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/unknown")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> MarkUnknown(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkOperationalStatusUnknownCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationOperationalStatusUnknownAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{orchestrationId:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleCommandResponse>>> DeleteLifecycle(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToDeleteLifecycleCommand(orchestrationId);
        var result = await _lifecycleHandler.DeleteOrchestrationLifecycleAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    // =====================================================================
    // ORCHESTRATION LIFECYCLE OPERATION COMMANDS
    [HttpPost("{orchestrationId:guid}/lifecycle/operations")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> CreateOperation(
        Guid orchestrationId, [FromBody] CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCreateOperationCommand(orchestrationId, request);
        var result = await _operationHandler.CreateOrchestrationLifecycleOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/execute")]
    [ProducesResponseType(
    typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>),
    StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> ExecuteRuntimeOperation(
        Guid orchestrationId, [FromBody] CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCreateOperationCommand(orchestrationId, request);
        var result = await _operationHandler.ExecuteOrchestrationRuntimeOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> StartOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToStartOperationCommand(operationId);
        var result = await _operationHandler.StartOrchestrationLifecycleOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> CompleteOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCompleteOperationCommand(operationId);
        var result = await _operationHandler.CompleteOrchestrationLifecycleOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/fail")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> FailOperation(
        Guid operationId, [FromBody] FailOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToFailOperationCommand(operationId, request);
        var result = await _operationHandler.FailOrchestrationLifecycleOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> CancelOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCancelOperationCommand(operationId);
        var result = await _operationHandler.CancelOrchestrationLifecycleOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("lifecycle-operations/{operationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationCommandResponse>>> DeleteOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToDeleteOperationCommand(operationId);
        var result = await _operationHandler.DeleteOrchestrationLifecycleOperationAsync(command, cancellationToken);
        var response = OrchestrationManagementResponseCommandMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}