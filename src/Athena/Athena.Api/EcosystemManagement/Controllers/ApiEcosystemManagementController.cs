using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Application.EcosystemManagement.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.EcosystemManagement.Controllers;

[ApiController]
[Route("api/ecosystems/management")]
public sealed class ApiEcosystemManagementController(IEcosystemManagement management) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EcosystemLifecycleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<EcosystemLifecycleDto>>>> GetAll(CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.GetLifecyclesAsync(token)));

    [HttpGet("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemLifecycleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<EcosystemLifecycleDto>>> GetLifecycle(Guid id, CancellationToken token)
    {
        var result = await management.GetLifecycleAsync(id, token);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPost("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemLifecycleDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<EcosystemLifecycleDto>>> CreateLifecycle(Guid id, CancellationToken token)
    {
        var result = await management.CreateLifecycleAsync(id, token);
        return CreatedAtAction(nameof(GetLifecycle), new { id }, ApiResponseBuilder.BuildSuccess(result));
    }

    [HttpPatch("{id:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemLifecycleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<EcosystemLifecycleDto>>> SetDesiredState(
        Guid id, [FromBody] SetManagementDesiredStateRequest request, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.SetDesiredStateAsync(id, request.DesiredState, token)));

    [HttpPost("{id:guid}/operations/{operationType}")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemOperationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<EcosystemOperationDto>>> ExecuteOperation(
        Guid id, string operationType, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.ExecuteOperationAsync(id, operationType, token)));

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EcosystemOperationDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<EcosystemOperationDto>>>> GetOperations(Guid id, CancellationToken token) =>
        Ok(ApiResponseBuilder.BuildSuccess(await management.GetOperationsAsync(id, token)));
}
