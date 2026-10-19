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

    // =============================================================================================================
    // ORCHESTRATION LIFECYCLE COMMANDS
    [HttpPost("{orchestrationId:guid}/lifecycle")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> CreateLifecycle(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var result = await _lifecycleHandler.CreateOrchestrationLifecycleAsync(orchestrationId, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPatch("{orchestrationId:guid}/lifecycle/desired-state")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> SetDesiredState(
        Guid orchestrationId, [FromBody] SetOrchestrationDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToSetDesiredStateCommand(orchestrationId, request);
        var result = await _lifecycleHandler.SetOrchestrationDesiredStateAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/starting")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> MarkStarting(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkStartingCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationStartingAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/running")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> MarkRunning(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkRunningCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationRunningAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/stopping")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> MarkStopping(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkStoppingCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationStoppingAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/stopped")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> MarkStopped(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkStoppedCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationStoppedAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/failed")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> MarkFailed(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkFailedCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationFailedAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("{orchestrationId:guid}/lifecycle/unknown")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> MarkUnknown(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToMarkOperationalStatusUnknownCommand(orchestrationId);
        var result = await _lifecycleHandler.MarkOrchestrationOperationalStatusUnknownAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpDelete("{orchestrationId:guid}/lifecycle")]
    public async Task<ActionResult<OrchestrationLifecycleCommandResponse>> DeleteLifecycle(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleRequestCommandMapper.ToDeleteLifecycleCommand(orchestrationId);
        var result = await _lifecycleHandler.DeleteOrchestrationLifecycleAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    // =============================================================================================================
    // ORCHESTRATION LIFECYCLE OPERATION COMMANDS
    [HttpPost("{orchestrationId:guid}/lifecycle/operations")]
    public async Task<ActionResult<OrchestrationLifecycleOperationCommandResponse>> CreateOperation(
        Guid orchestrationId, [FromBody] CreateOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCreateOperationCommand(orchestrationId, request);
        var result = await _operationHandler.CreateOrchestrationLifecycleOperationAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/start")]
    public async Task<ActionResult<OrchestrationLifecycleOperationCommandResponse>> StartOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToStartOperationCommand(operationId);
        var result = await _operationHandler.StartOrchestrationLifecycleOperationAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/complete")]
    public async Task<ActionResult<OrchestrationLifecycleOperationCommandResponse>> CompleteOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCompleteOperationCommand(operationId);
        var result = await _operationHandler.CompleteOrchestrationLifecycleOperationAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/fail")]
    public async Task<ActionResult<OrchestrationLifecycleOperationCommandResponse>> FailOperation(
        Guid operationId, [FromBody] FailOrchestrationLifecycleOperationRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToFailOperationCommand(operationId, request);
        var result = await _operationHandler.FailOrchestrationLifecycleOperationAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpPost("lifecycle-operations/{operationId:guid}/cancel")]
    public async Task<ActionResult<OrchestrationLifecycleOperationCommandResponse>> CancelOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToCancelOperationCommand(operationId);
        var result = await _operationHandler.CancelOrchestrationLifecycleOperationAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }

    [HttpDelete("lifecycle-operations/{operationId:guid}")]
    public async Task<ActionResult<OrchestrationLifecycleOperationCommandResponse>> DeleteOperation(
        Guid operationId, CancellationToken cancellationToken)
    {
        var command = OrchestrationLifecycleOperationRequestCommandMapper.ToDeleteOperationCommand(operationId);
        var result = await _operationHandler.DeleteOrchestrationLifecycleOperationAsync(command, cancellationToken);
        return Ok(OrchestrationManagementResponseCommandMapper.ToResponse(result));
    }
}