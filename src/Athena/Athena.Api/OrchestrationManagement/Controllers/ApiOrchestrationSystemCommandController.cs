using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.OrchestrationManagement.Mappings;
using Athena.Api.OrchestrationManagement.Requests;
using Athena.Api.OrchestrationManagement.Responses;
using Athena.Application.OrchestrationManagement.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.OrchestrationManagement.Controllers;

[ApiController]
[Route("api/orchestrations/management/systems")]
public class OrchestrationSystemCommandController(OrchestrationSystemUseCaseHandler handler) : ControllerBase
{
    private readonly OrchestrationSystemUseCaseHandler _handler = handler;

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemCommandResponse>>> Create([FromBody] CreateOrchestrationSystemRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationSystemRequestCommandMapper.ToCreateCommand(request);
        var result = await _handler.CreateOrchestrationSystemAsync(command, cancellationToken);
        var response = OrchestrationSystemResponseMapper.ToCommandResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemCommandResponse>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationSystemRequestCommandMapper.ToDeleteCommand(id);
        var result = await _handler.DeleteOrchestrationSystemAsync(command, cancellationToken);
        var response = OrchestrationSystemResponseMapper.ToCommandResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemCommandResponse>>> Restore(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationSystemRequestCommandMapper.ToRestoreCommand(id);
        var result = await _handler.RestoreOrchestrationSystemAsync(command, cancellationToken);
        var response = OrchestrationSystemResponseMapper.ToCommandResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
