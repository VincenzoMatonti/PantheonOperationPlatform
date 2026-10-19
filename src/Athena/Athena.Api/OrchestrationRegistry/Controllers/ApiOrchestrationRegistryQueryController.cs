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

    // =====================================================================
    // QUERIES
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByIdQuery(id);
        var result = await _handler.GetByIdAsync(query, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("by-name")]
    [ProducesResponseType(typeof(OrchestrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrchestrationResponse>> GetByName([FromQuery] GetOrchestrationByNameRequest request, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByNameQuery(request);
        var result = await _handler.GetByNameAsync(query, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrchestrationResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _handler.GetAllAsync(cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponses(result));
    }

    [HttpGet("by-registration-status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyList<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrchestrationResponse>>> GetByRegistrationStatus(OrchestrationRegistrationStatusFilter status, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByRegistrationStatusQuery(status);
        var result = await _handler.GetByRegistrationStatusAsync(query, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponses(result));
    }

    [HttpGet("by-availability-status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyList<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrchestrationResponse>>> GetByAvailabilityStatus(OrchestrationAvailabilityStatusFilter status, CancellationToken cancellationToken)
    {
        var query = OrchestrationRegistryQueryRequestMapper.ToByAvailabilityStatusQuery(status);
        var result = await _handler.GetByAvailabilityStatusAsync(query, cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponses(result));
    }

    [HttpGet("deleted")]
    [ProducesResponseType(typeof(IReadOnlyList<OrchestrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrchestrationResponse>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await _handler.GetDeletedAsync(cancellationToken);
        return Ok(OrchestrationRegistryResponseMapper.ToResponses(result));
    }
}