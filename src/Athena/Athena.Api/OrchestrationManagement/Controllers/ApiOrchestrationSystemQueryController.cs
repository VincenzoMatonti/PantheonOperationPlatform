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
public class OrchestrationSystemInstanceQueryController(OrchestrationSystemInstanceUseCaseHandler handler) : ControllerBase
{
    private readonly OrchestrationSystemInstanceUseCaseHandler _handler = handler;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationSystemInstanceQueryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationSystemInstanceQueryResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = OrchestrationSystemInstanceRequestQueryMapper.ToGetByIdQuery(id);
        var result = await _handler.GetOrchestrationSystemInstanceByIdAsync(query, cancellationToken);
        var response = OrchestrationSystemInstanceResponseMapper.ToQueryResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationSystemInstanceQueryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationSystemInstanceQueryResponse>>>> GetAll([FromQuery] GetOrchestrationSystemInstancesRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationSystemInstanceRequestQueryMapper.ToGetAllQuery(request);
        var result = await _handler.GetAllOrchestrationSystemInstancesAsync(query, cancellationToken);
        var response = OrchestrationSystemInstanceResponseMapper.ToQueryResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}
