using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.WorkloadManagement.Mappings;
using Athena.Api.WorkloadManagement.Requests;
using Athena.Api.WorkloadManagement.Responses;
using Athena.Application.WorkloadManagement.Contracts;
using Athena.Domain.Workload.Management.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.WorkloadManagement.Controllers;

[ApiController]
[Route("api/workloads/management")]
public sealed class ApiWorkloadManagementController(IWorkloadManagement management) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WorkloadLifecycleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkloadLifecycleResponse>>>> GetAll(CancellationToken token)
    {
        var query = WorkloadManagementRequestMapper.ToGetLifecyclesQuery();
        var result = await management.GetLifecyclesAsync(query, token);
        return Ok(ApiResponseBuilder.BuildSuccess(WorkloadManagementResponseMapper.ToResponses(result)));
    }

    [HttpGet("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadLifecycleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WorkloadLifecycleResponse>>> GetLifecycle(Guid id, CancellationToken token)
    {
        var query = WorkloadManagementRequestMapper.ToGetLifecycleQuery(id);
        var result = await management.GetLifecycleAsync(query, token);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(WorkloadManagementResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadLifecycleResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<WorkloadLifecycleResponse>>> CreateLifecycle(Guid id, CancellationToken token)
    {
        var command = WorkloadManagementRequestMapper.ToCreateLifecycleCommand(id);
        var result = await management.CreateLifecycleAsync(command, token);
        var response = WorkloadManagementResponseMapper.ToResponse(result);
        return CreatedAtAction(nameof(GetLifecycle), new { id }, ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{id:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadLifecycleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<WorkloadLifecycleResponse>>> SetDesiredState(
        Guid id, [FromBody] SetWorkloadDesiredStateRequest request, CancellationToken token)
    {
        var command = WorkloadManagementRequestMapper.ToSetDesiredStateCommand(id, request);
        var result = await management.SetDesiredStateAsync(command, token);
        return Ok(ApiResponseBuilder.BuildSuccess(WorkloadManagementResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/operations/{operationType}")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadOperationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<WorkloadOperationResponse>>> ExecuteOperation(
        Guid id, WorkloadOperationType operationType, CancellationToken token)
    {
        var command = WorkloadManagementRequestMapper.ToExecuteOperationCommand(id, operationType);
        var result = await management.ExecuteOperationAsync(command, token);
        return Ok(ApiResponseBuilder.BuildSuccess(WorkloadManagementResponseMapper.ToResponse(result)));
    }

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WorkloadOperationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkloadOperationResponse>>>> GetOperations(Guid id, CancellationToken token)
    {
        var query = WorkloadManagementRequestMapper.ToGetOperationsQuery(id);
        var result = await management.GetOperationsAsync(query, token);
        return Ok(ApiResponseBuilder.BuildSuccess(WorkloadManagementResponseMapper.ToOperationResponses(result)));
    }
}
