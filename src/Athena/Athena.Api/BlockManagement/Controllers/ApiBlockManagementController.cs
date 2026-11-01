using Athena.Api.BlockManagement.Mappings;
using Athena.Api.BlockManagement.Requests;
using Athena.Api.BlockManagement.Responses;
using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Application.BlockManagement.Contracts;
using Athena.Domain.Block.Management.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.BlockManagement.Controllers;

[ApiController]
[Route("api/blocks/management")]
public sealed class ApiBlockManagementController(IBlockManagement management) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlockLifecycleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BlockLifecycleResponse>>>> GetAll(CancellationToken token)
    {
        var query = BlockManagementRequestMapper.ToGetLifecyclesQuery();
        var result = await management.GetLifecyclesAsync(query, token);
        return Ok(ApiResponseBuilder.BuildSuccess(BlockManagementResponseMapper.ToResponses(result)));
    }

    [HttpGet("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<BlockLifecycleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BlockLifecycleResponse>>> GetLifecycle(Guid id, CancellationToken token)
    {
        var query = BlockManagementRequestMapper.ToGetLifecycleQuery(id);
        var result = await management.GetLifecycleAsync(query, token);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(BlockManagementResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<BlockLifecycleResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<BlockLifecycleResponse>>> CreateLifecycle(Guid id, CancellationToken token)
    {
        var command = BlockManagementRequestMapper.ToCreateLifecycleCommand(id);
        var result = await management.CreateLifecycleAsync(command, token);
        var response = BlockManagementResponseMapper.ToResponse(result);
        return CreatedAtAction(nameof(GetLifecycle), new { id }, ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{id:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<BlockLifecycleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BlockLifecycleResponse>>> SetDesiredState(
        Guid id, [FromBody] SetBlockDesiredStateRequest request, CancellationToken token)
    {
        var command = BlockManagementRequestMapper.ToSetDesiredStateCommand(id, request);
        var result = await management.SetDesiredStateAsync(command, token);
        return Ok(ApiResponseBuilder.BuildSuccess(BlockManagementResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/operations/{operationType}")]
    [ProducesResponseType(typeof(ApiResponse<BlockOperationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BlockOperationResponse>>> ExecuteOperation(
        Guid id, BlockOperationType operationType, CancellationToken token)
    {
        var command = BlockManagementRequestMapper.ToExecuteOperationCommand(id, operationType);
        var result = await management.ExecuteOperationAsync(command, token);
        return Ok(ApiResponseBuilder.BuildSuccess(BlockManagementResponseMapper.ToResponse(result)));
    }

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlockOperationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BlockOperationResponse>>>> GetOperations(Guid id, CancellationToken token)
    {
        var query = BlockManagementRequestMapper.ToGetOperationsQuery(id);
        var result = await management.GetOperationsAsync(query, token);
        return Ok(ApiResponseBuilder.BuildSuccess(BlockManagementResponseMapper.ToOperationResponses(result)));
    }
}
