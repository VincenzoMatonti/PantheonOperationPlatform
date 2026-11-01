using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.EcosystemManagement.Mappings;
using Athena.Api.EcosystemManagement.Requests;
using Athena.Api.EcosystemManagement.Responses;
using Athena.Application.EcosystemManagement.Contracts;
using Athena.Domain.Ecosystem.Management.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.EcosystemManagement.Controllers;

[ApiController]
[Route("api/ecosystems/management")]
public sealed class ApiEcosystemManagementController(IEcosystemManagement management) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EcosystemLifecycleResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<EcosystemLifecycleResponse>>>> GetAll(CancellationToken token)
    {
        var query = EcosystemManagementRequestMapper.ToGetLifecyclesQuery();
        var result = await management.GetLifecyclesAsync(query, token);
        return Ok(ApiResponseBuilder.BuildSuccess(EcosystemManagementResponseMapper.ToResponses(result)));
    }

    [HttpGet("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemLifecycleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<EcosystemLifecycleResponse>>> GetLifecycle(Guid id, CancellationToken token)
    {
        var query = EcosystemManagementRequestMapper.ToGetLifecycleQuery(id);
        var result = await management.GetLifecycleAsync(query, token);
        return result is null ? NotFound() : Ok(ApiResponseBuilder.BuildSuccess(EcosystemManagementResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/lifecycle")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemLifecycleResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<EcosystemLifecycleResponse>>> CreateLifecycle(Guid id, CancellationToken token)
    {
        var command = EcosystemManagementRequestMapper.ToCreateLifecycleCommand(id);
        var result = await management.CreateLifecycleAsync(command, token);
        var response = EcosystemManagementResponseMapper.ToResponse(result);
        return CreatedAtAction(nameof(GetLifecycle), new { id }, ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPatch("{id:guid}/lifecycle/desired-state")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemLifecycleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<EcosystemLifecycleResponse>>> SetDesiredState(
        Guid id, [FromBody] SetEcosystemDesiredStateRequest request, CancellationToken token)
    {
        var command = EcosystemManagementRequestMapper.ToSetDesiredStateCommand(id, request);
        var result = await management.SetDesiredStateAsync(command, token);
        return Ok(ApiResponseBuilder.BuildSuccess(EcosystemManagementResponseMapper.ToResponse(result)));
    }

    [HttpPost("{id:guid}/operations/{operationType}")]
    [ProducesResponseType(typeof(ApiResponse<EcosystemOperationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<EcosystemOperationResponse>>> ExecuteOperation(
        Guid id, EcosystemOperationType operationType, CancellationToken token)
    {
        var command = EcosystemManagementRequestMapper.ToExecuteOperationCommand(id, operationType);
        var result = await management.ExecuteOperationAsync(command, token);
        return Ok(ApiResponseBuilder.BuildSuccess(EcosystemManagementResponseMapper.ToResponse(result)));
    }

    [HttpGet("{id:guid}/operations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EcosystemOperationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<EcosystemOperationResponse>>>> GetOperations(Guid id, CancellationToken token)
    {
        var query = EcosystemManagementRequestMapper.ToGetOperationsQuery(id);
        var result = await management.GetOperationsAsync(query, token);
        return Ok(ApiResponseBuilder.BuildSuccess(EcosystemManagementResponseMapper.ToOperationResponses(result)));
    }
}
