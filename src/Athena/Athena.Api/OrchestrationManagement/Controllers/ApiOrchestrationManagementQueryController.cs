using Athena.Api.OrchestrationManagement.Mappings;
using Athena.Api.OrchestrationManagement.Requests;
using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.OrchestrationManagement.Controllers;

[ApiController]
[Route("api/orchestrations/management")]
public class OrchestrationManagementQueryController(
    OrchestrationLifecycleUseCaseHandler lifecycleHandler,
    OrchestrationLifecycleOperationUseCaseHandler operationHandler) : ControllerBase
{
    private readonly OrchestrationLifecycleUseCaseHandler _lifecycleHandler = lifecycleHandler;
    private readonly OrchestrationLifecycleOperationUseCaseHandler _operationHandler = operationHandler;

    // =============================================================================================================
    // ORCHESTRATION LIFECYCLE QUERIES
    [HttpGet("{orchestrationId:guid}/lifecycle")]
    public async Task<ActionResult<OrchestrationLifecycleQueryResponse>> GetLifecycleById(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleRequestQueryMapper.ToGetByIdQuery(orchestrationId);
        var result = await _lifecycleHandler.GetOrchestrationLifecycleByIdAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToResponse(result));
    }

    [HttpGet("lifecycles")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleQueryResponse>>> GetAllLifecycles(CancellationToken cancellationToken)
    {
        var result = await _lifecycleHandler.GetAllOrchestrationLifecyclesAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/by-desired-state")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleQueryResponse>>> GetLifecyclesByDesiredState(
        [FromQuery] GetOrchestrationLifecyclesByDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleRequestQueryMapper.ToGetByDesiredStateQuery(request);
        var result = await _lifecycleHandler.GetOrchestrationLifecyclesByDesiredStateAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/by-operational-status")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleQueryResponse>>> GetLifecyclesByOperationalStatus(
        [FromQuery] GetOrchestrationLifecyclesByOperationalStatusRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleRequestQueryMapper.ToGetByOperationalStatusQuery(request);
        var result = await _lifecycleHandler.GetOrchestrationLifecyclesByOperationalStatusAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/deleted")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleQueryResponse>>> GetDeletedLifecycles(CancellationToken cancellationToken)
    {
        var result = await _lifecycleHandler.GetDeletedOrchestrationLifecyclesAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    // =============================================================================================================
    // ORCHESTRATION LIFECYCLE OPERATION QUERIES
    [HttpGet("lifecycle-operations/{operationId:guid}")]
    public async Task<ActionResult<OrchestrationLifecycleOperationQueryResponse>> GetOperationById(
        Guid operationId, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByIdQuery(operationId);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationByIdAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToResponse(result));
    }

    [HttpGet("{orchestrationId:guid}/lifecycle/operations")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetOperationsByOrchestrationId(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var request = new GetOrchestrationLifecycleOperationsByOrchestrationIdRequest { OrchestrationId = orchestrationId };
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByOrchestrationIdQuery(request);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByOrchestrationIdAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetAllOperations(CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetAllOrchestrationLifecycleOperationsAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/by-type")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetOperationsByType(
        [FromQuery] GetOrchestrationLifecycleOperationsByTypeRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByTypeQuery(request);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByTypeAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/by-status")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetOperationsByStatus(
        [FromQuery] GetOrchestrationLifecycleOperationsByStatusRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByStatusQuery(request);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByStatusAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("{orchestrationId:guid}/lifecycle/operations/by-status")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetOperationsByOrchestrationIdAndStatus(
        Guid orchestrationId,
        [FromQuery] GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest request,
        CancellationToken cancellationToken)
    {
        var queryRequest = new GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest { OrchestrationId = orchestrationId, Status = request.Status };
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByOrchestrationIdAndStatusQuery(queryRequest);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusAsync(query, cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/pending")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetPendingOperations(CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetPendingOrchestrationLifecycleOperationsAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/running")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetRunningOperations(CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetRunningOrchestrationLifecycleOperationsAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/failed")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetFailedOperations(CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetFailedOrchestrationLifecycleOperationsAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/deleted")]
    public async Task<ActionResult<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>> GetDeletedOperations(CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetDeletedOrchestrationLifecycleOperationsAsync(cancellationToken);
        return Ok(OrchestrationManagementResponseQueryMapper.ToOperationResponses(result));
    }
}