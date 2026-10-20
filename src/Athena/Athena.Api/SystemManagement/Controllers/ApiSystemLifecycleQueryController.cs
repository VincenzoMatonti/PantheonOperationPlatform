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
public class SystemLifecycleQueryController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpGet("{systemId:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<SystemLifecycleQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemLifecycleQueryResponse>>> GetBySystemId(Guid systemId, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecycleBySystemIdQuery(systemId);
        var result = await handler.GetSystemLifecycleBySystemIdAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await handler.GetAllSystemLifecyclesAsync(cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/by-desired-state")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>>> GetByDesiredState(
        [FromQuery] GetSystemLifecyclesByDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecyclesByDesiredStateQuery(request);
        var result = await handler.GetSystemLifecyclesByDesiredStateAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/by-operational-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>>> GetByOperationalStatus(
        [FromQuery] GetSystemLifecyclesByOperationalStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecyclesByOperationalStatusQuery(request);
        var result = await handler.GetSystemLifecyclesByOperationalStatusAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/by-deletion-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>>> GetByDeletionStatus(
        [FromQuery] GetSystemLifecyclesByDeletionStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecyclesByDeletionStatusQuery(request);
        var result = await handler.GetSystemLifecyclesByDeletionStatusAsync(query, cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("lifecycles/deleted")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemLifecycleQueryResponse>>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await handler.GetDeletedSystemLifecyclesAsync(cancellationToken);
        var response = SystemManagementResponseQueryMapper.ToLifecycleResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
