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
public class SystemLifecycleOperationQueryController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpGet("lifecycle-operations/{operationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleOperationQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleOperationQueryResponse>>> GetById(Guid operationId, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationByIdQuery(operationId);
        var result = await handler.GetSystemLifecycleOperationByIdAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{systemId:guid}/lifecycle/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>>> GetBySystemId(
        Guid systemId, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsBySystemIdQuery(systemId);
        var result = await handler.GetSystemLifecycleOperationsBySystemIdAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/by-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>>> GetByStatus(
        [FromQuery] GetSystemLifecycleOperationsByStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsByStatusQuery(request);
        var result = await handler.GetSystemLifecycleOperationsByStatusAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{systemId:guid}/lifecycle/operations/by-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>>> GetBySystemIdAndStatus(
        Guid systemId, [FromQuery] GetSystemLifecycleOperationsBySystemIdAndStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsBySystemIdAndStatusQuery(systemId, request);
        var result = await handler.GetSystemLifecycleOperationsBySystemIdAndStatusAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/by-deletion-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>>> GetByDeletionStatus(
        [FromQuery] GetSystemLifecycleOperationsByDeletionStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsByDeletionStatusQuery(request);
        var result = await handler.GetSystemLifecycleOperationsByDeletionStatusAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycle-operations/deleted")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleOperationQueryResponse>>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await handler.GetDeletedSystemLifecycleOperationsAsync(cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToOperationResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}