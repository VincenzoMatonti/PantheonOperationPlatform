using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.OrchestrationRegistry.Mappings;
using Athena.Api.OrchestrationRegistry.Requests;
using Athena.Api.OrchestrationRegistry.Responses;
using Athena.Application.OrchestrationRegistry.Enums;
using Athena.Application.OrchestrationRegistry.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.OrchestrationRegistry.Controllers;

[ApiController]
[Route("api/orchestrations")]
public sealed class OrchestrationRegistryQueryController(OrchestrationRegistryUseCaseHandler handler) : ControllerBase
{
    private readonly OrchestrationRegistryUseCaseHandler _handler = handler;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByIdQuery(id);
        var result = await _handler.GetByIdAsync(query, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-name")]
    [ProducesResponseType(typeof(ApiResponse<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<OrchestrationResponse>>> GetByName(
        [FromQuery] GetOrchestrationByNameRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByNameQuery(request);
        var result = await _handler.GetByNameAsync(query, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _handler.GetAllAsync(cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-registration-status/{status}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationResponse>>>> GetByRegistrationStatus(
        OrchestrationRegistrationStatusFilter status, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByRegistrationStatusQuery(status);
        var result = await _handler.GetByRegistrationStatusAsync(query, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-availability-status/{status}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationResponse>>>> GetByAvailabilityStatus(
        OrchestrationAvailabilityStatusFilter status, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByAvailabilityStatusQuery(status);
        var result = await _handler.GetByAvailabilityStatusAsync(query, cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("deleted")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrchestrationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrchestrationResponse>>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await _handler.GetDeletedAsync(cancellationToken);
        var response = OrchestrationRegistryResponseMapper.ToResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}