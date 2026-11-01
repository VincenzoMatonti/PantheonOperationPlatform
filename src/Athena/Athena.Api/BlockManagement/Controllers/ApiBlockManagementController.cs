using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Application.BlockManagement.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.BlockManagement.Controllers;

[ApiController]
[Route("api/blocks/management")]
public sealed class ApiBlockManagementController(IBlockManagement management) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlockLifecycleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BlockLifecycleDto>>>> GetAll(CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.GetLifecyclesAsync(token)));

    [HttpGet("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<BlockLifecycleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BlockLifecycleDto>>> GetLifecycle(Guid id, CancellationToken token)
    {
        var result = await management.GetLifecycleAsync(id, token);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<BlockLifecycleDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<BlockLifecycleDto>>> CreateLifecycle(Guid id, CancellationToken token)
    {
        var result = await management.CreateLifecycleAsync(id, token);
        return CreatedAtAction(nameof(GetLifecycle), new { id }, ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPatch("{id:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<BlockLifecycleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BlockLifecycleDto>>> SetDesiredState(
        Guid id, [FromBody] SetManagementDesiredStateRequest request, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.SetDesiredStateAsync(id, request.DesiredState, token)));

    [HttpPost("{id:guid}/operations/{operationType}")]
    [ProducesResponseType(typeof(ApiResponse<BlockOperationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BlockOperationDto>>> ExecuteOperation(
        Guid id, string operationType, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.ExecuteOperationAsync(id, operationType, token)));

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlockOperationDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BlockOperationDto>>>> GetOperations(Guid id, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.GetOperationsAsync(id, token)));
}
