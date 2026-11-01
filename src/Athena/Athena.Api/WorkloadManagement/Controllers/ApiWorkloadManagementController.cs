using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Application.WorkloadManagement.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.WorkloadManagement.Controllers;

[ApiController]
[Route("api/workloads/management")]
public sealed class ApiWorkloadManagementController(IWorkloadManagement management) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WorkloadLifecycleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkloadLifecycleDto>>>> GetAll(CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.GetLifecyclesAsync(token)));

    [HttpGet("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadLifecycleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WorkloadLifecycleDto>>> GetLifecycle(Guid id, CancellationToken token)
    {
        var result = await management.GetLifecycleAsync(id, token);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadLifecycleDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<WorkloadLifecycleDto>>> CreateLifecycle(Guid id, CancellationToken token)
    {
        var result = await management.CreateLifecycleAsync(id, token);
        return CreatedAtAction(nameof(GetLifecycle), new { id }, ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPatch("{id:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadLifecycleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<WorkloadLifecycleDto>>> SetDesiredState(
        Guid id, [FromBody] SetManagementDesiredStateRequest request, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.SetDesiredStateAsync(id, request.DesiredState, token)));

    [HttpPost("{id:guid}/operations/{operationType}")]
    [ProducesResponseType(typeof(ApiResponse<WorkloadOperationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<WorkloadOperationDto>>> ExecuteOperation(
        Guid id, string operationType, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.ExecuteOperationAsync(id, operationType, token)));

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WorkloadOperationDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkloadOperationDto>>>> GetOperations(Guid id, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.GetOperationsAsync(id, token)));
}
