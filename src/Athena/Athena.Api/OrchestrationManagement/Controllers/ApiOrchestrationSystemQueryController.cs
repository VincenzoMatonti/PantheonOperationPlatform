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
public class OrchestrationSystemQueryController(OrchestrationSystemUseCaseHandler handler) : ControllerBase
{
    private readonly OrchestrationSystemUseCaseHandler _handler = handler;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemQueryResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = OrchestrationSystemRequestQueryMapper.ToGetByIdQuery(id);
        var result = await _handler.GetOrchestrationSystemByIdAsync(query, cancellationToken);
        var response = OrchestrationSystemResponseMapper.ToQueryResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationSystemQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationSystemQueryResponse>>>> GetAll([FromQuery] GetOrchestrationSystemsRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationSystemRequestQueryMapper.ToGetAllQuery(request);
        var result = await _handler.GetAllOrchestrationSystemsAsync(query, cancellationToken);
        var response = OrchestrationSystemResponseMapper.ToQueryResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
