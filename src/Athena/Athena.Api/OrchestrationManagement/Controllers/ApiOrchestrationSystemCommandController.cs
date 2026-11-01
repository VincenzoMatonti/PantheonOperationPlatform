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
[Route("api/orchestrations/management/system-instances")]
public class OrchestrationSystemInstanceCommandController(OrchestrationSystemInstanceUseCaseHandler handler) : ControllerBase
{
    private readonly OrchestrationSystemInstanceUseCaseHandler _handler = handler;

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemInstanceCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemInstanceCommandResponse>>> Create([FromBody] CreateOrchestrationSystemInstanceRequest request, CancellationToken cancellationToken)
    {
        var command = OrchestrationSystemInstanceRequestCommandMapper.ToCreateCommand(request);
        var result = await _handler.CreateOrchestrationSystemInstanceAsync(command, cancellationToken);
        var response = OrchestrationSystemInstanceResponseMapper.ToCommandResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemInstanceCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemInstanceCommandResponse>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationSystemInstanceRequestCommandMapper.ToDeleteCommand(id);
        var result = await _handler.DeleteOrchestrationSystemInstanceAsync(command, cancellationToken);
        var response = OrchestrationSystemInstanceResponseMapper.ToCommandResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemInstanceCommandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemInstanceCommandResponse>>> Restore(Guid id, CancellationToken cancellationToken)
    {
        var command = OrchestrationSystemInstanceRequestCommandMapper.ToRestoreCommand(id);
        var result = await _handler.RestoreOrchestrationSystemInstanceAsync(command, cancellationToken);
        var response = OrchestrationSystemInstanceResponseMapper.ToCommandResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
