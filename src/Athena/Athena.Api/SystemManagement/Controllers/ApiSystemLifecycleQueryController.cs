using Athena.Api.SystemManagement.Mappings;
using Athena.Api.SystemManagement.Requests;
using Athena.Api.SystemManagement.Responses;
using Athena.Application.SystemManagement.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemManagement.Controllers;

[ApiController]
[Route("api/systems/mangement")]
public class SystemLifecycleQueryController(SystemManagementUseCaseHandler handler) : ControllerBase
{
    [HttpGet("{systemId:guid}/lifecycle")]
    public async Task<ActionResult<SystemLifecycleQueryResponse>> GetBySystemId(Guid systemId, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecycleBySystemIdQuery(systemId);
        var result = await handler.GetSystemLifecycleBySystemIdAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToResponse(result));
    }

    [HttpGet("lifecycles")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleQueryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await handler.GetAllSystemLifecyclesAsync(cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/by-desired-state")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleQueryResponse>>> GetByDesiredState(
        [FromQuery] GetSystemLifecyclesByDesiredStateRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecyclesByDesiredStateQuery(request);
        var result = await handler.GetSystemLifecyclesByDesiredStateAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/by-operational-status")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleQueryResponse>>> GetByOperationalStatus(
        [FromQuery] GetSystemLifecyclesByOperationalStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecyclesByOperationalStatusQuery(request);
        var result = await handler.GetSystemLifecyclesByOperationalStatusAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/by-deletion-status")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleQueryResponse>>> GetByDeletionStatus(
        [FromQuery] GetSystemLifecyclesByDeletionStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemManagementRequestQueryMapper.ToLifecyclesByDeletionStatusQuery(request);
        var result = await handler.GetSystemLifecyclesByDeletionStatusAsync(query, cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToLifecycleResponses(result));
    }

    [HttpGet("lifecycles/deleted")]
    public async Task<ActionResult<IReadOnlyList<SystemLifecycleQueryResponse>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await handler.GetDeletedSystemLifecyclesAsync(cancellationToken);
        return Ok(SystemManagementResponseQueryMapper.ToLifecycleResponses(result));
    }
}