using Athena.Api.SystemManagement.Mappings;
using Athena.Api.SystemManagement.Requests;
using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/systems/mangement")]
public class SystemLifecycleOperationQueryController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpGet("lifecycle-operations/{operationId:guid}")]
    public async Task<ActionResult<SystemLifecycleOperationQueryResponse>> GetById(Guid operationId, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationByIdQuery(operationId);
        var result = await handler.GetSystemLifecycleOperationByIdAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToResponse(result));
    }

    [HttpGet("{systemId:guid}/lifecycle/operations")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleOperationQueryResponse>>> GetBySystemId(
        Guid systemId, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsBySystemIdQuery(systemId);
        var result = await handler.GetSystemLifecycleOperationsBySystemIdAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/by-status")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleOperationQueryResponse>>> GetByStatus(
        [FromQuery] GetSystemLifecycleOperationsByStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsByStatusQuery(request);
        var result = await handler.GetSystemLifecycleOperationsByStatusAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("{systemId:guid}/lifecycle/operations/by-status")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleOperationQueryResponse>>> GetBySystemIdAndStatus(
        Guid systemId, [FromQuery] GetSystemLifecycleOperationsBySystemIdAndStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsBySystemIdAndStatusQuery(systemId, request);
        var result = await handler.GetSystemLifecycleOperationsBySystemIdAndStatusAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/by-deletion-status")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleOperationQueryResponse>>> GetByDeletionStatus(
        [FromQuery] GetSystemLifecycleOperationsByDeletionStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToOperationsByDeletionStatusQuery(request);
        var result = await handler.GetSystemLifecycleOperationsByDeletionStatusAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToOperationResponses(result));
    }

    [HttpGet("lifecycle-operations/deleted")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleOperationQueryResponse>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await handler.GetDeletedSystemLifecycleOperationsAsync(cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToOperationResponses(result));
    }
}