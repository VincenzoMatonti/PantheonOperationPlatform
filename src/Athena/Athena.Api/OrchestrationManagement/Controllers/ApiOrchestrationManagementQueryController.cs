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
public class OrchestrationManagementQueryController(
    OrchestrationLifecycleUseCaseHandler lifecycleHandler,
    OrchestrationLifecycleOperationUseCaseHandler operationHandler) : ControllerBase
{
    private readonly OrchestrationLifecycleUseCaseHandler _lifecycleHandler = lifecycleHandler;
    private readonly OrchestrationLifecycleOperationUseCaseHandler _operationHandler = operationHandler;

    // =====================================================================
    // ORCHESTRATION LIFECYCLE QUERIES
    [HttpGet("{orchestrationId:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleQueryResponse>>> GetLifecycleById(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleRequestQueryMapper.ToGetByIdQuery(orchestrationId);
        var result = await _lifecycleHandler.GetOrchestrationLifecycleByIdAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>>> GetAllLifecycles(
        CancellationToken cancellationToken)
    {
        var result = await _lifecycleHandler.GetAllOrchestrationLifecyclesAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/by-desired-state")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>>> GetLifecyclesByDesiredState(
        [FromQuery] GetOrchestrationLifecyclesByDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleRequestQueryMapper.ToGetByDesiredStateQuery(request);
        var result = await _lifecycleHandler.GetOrchestrationLifecyclesByDesiredStateAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/by-operational-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>>> GetLifecyclesByOperationalStatus(
        [FromQuery] GetOrchestrationLifecyclesByOperationalStatusRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleRequestQueryMapper.ToGetByOperationalStatusQuery(request);
        var result = await _lifecycleHandler.GetOrchestrationLifecyclesByOperationalStatusAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/deleted")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleQueryResponse>>>> GetDeletedLifecycles(
        CancellationToken cancellationToken)
    {
        var result = await _lifecycleHandler.GetDeletedOrchestrationLifecyclesAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    // =====================================================================
    // ORCHESTRATION LIFECYCLE OPERATION QUERIES
    [HttpGet("lifecycle-operations/{operationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationLifecycleOperationQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationLifecycleOperationQueryResponse>>> GetOperationById(
        Guid operationId, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByIdQuery(operationId);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationByIdAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{orchestrationId:guid}/lifecycle/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetOperationsByOrchestrationId(
        Guid orchestrationId, CancellationToken cancellationToken)
    {
        var request = new GetOrchestrationLifecycleOperationsByOrchestrationIdRequest { OrchestrationId = orchestrationId };
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByOrchestrationIdQuery(request);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByOrchestrationIdAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetAllOperations(
        CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetAllOrchestrationLifecycleOperationsAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/by-type")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetOperationsByType(
        [FromQuery] GetOrchestrationLifecycleOperationsByTypeRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByTypeQuery(request);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByTypeAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/by-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetOperationsByStatus(
        [FromQuery] GetOrchestrationLifecycleOperationsByStatusRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByStatusQuery(request);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByStatusAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{orchestrationId:guid}/lifecycle/operations/by-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetOperationsByOrchestrationIdAndStatus(
        Guid orchestrationId, [FromQuery] GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest request, CancellationToken cancellationToken)
    {
        var queryRequest = new GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest
        {
            OrchestrationId = orchestrationId,
            Status = request.Status
        };

        var query = OrchestrationLifecycleOperationRequestQueryMapper.ToGetByOrchestrationIdAndStatusQuery(queryRequest);
        var result = await _operationHandler.GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusAsync(query, cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/pending")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetPendingOperations(
        CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetPendingOrchestrationLifecycleOperationsAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/running")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetRunningOperations(
        CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetRunningOrchestrationLifecycleOperationsAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/failed")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetFailedOperations(
        CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetFailedOrchestrationLifecycleOperationsAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/deleted")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationLifecycleOperationQueryResponse>>>> GetDeletedOperations(
        CancellationToken cancellationToken)
    {
        var result = await _operationHandler.GetDeletedOrchestrationLifecycleOperationsAsync(cancellationToken);
        var response = OrchestrationManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}