using Athena.Api.SystemRegistry.Mappings;
using Athena.Api.SystemRegistry.Requests;
using Athena.Api.SystemRegistry.Responses;
using Athena.Application.SystemRegistry.Enums;
using Athena.Application.SystemRegistry.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemRegistry.Controllers;

[ApiController]
[Route("api/systems")]
public class SystemRegistryQueryController(SystemRegistryUseCaseHandler handler) : ControllerBase
{
    private readonly SystemRegistryUseCaseHandler _handler = handler;

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToByIdQuery(id);
        var result = await _handler.GetSystemByIdAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("by-name")]
    public async Task<ActionResult<SystemResponse>> GetByName([FromQuery] GetSystemByNameRequest request, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(request);
        var result = await _handler.GetSystemByNameAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SystemResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _handler.GetAllSystemsAsync(cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("by-registration-status/{status}")]
    public async Task<ActionResult<IReadOnlyList<SystemResponse>>> GetByRegistrationStatus(SystemRegistrationStatusFilter status, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(status);
        var result = await _handler.GetSystemsByRegistrationStatusAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("by-availability-status/{status}")]
    public async Task<ActionResult<IReadOnlyList<SystemResponse>>> GetByAvailabilityStatus(SystemAvailabilityStatusFilter status, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(status);
        var result = await _handler.GetSystemsByAvailabilityStatusAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("by-capability")]
    public async Task<ActionResult<IReadOnlyList<SystemResponse>>> GetByCapability([FromQuery] GetSystemsByCapabilityRequest request, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(request);
        var result = await _handler.GetSystemsByCapabilityAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("{id:guid}/endpoints")]
    public async Task<ActionResult<IReadOnlyList<SystemEndpointResponse>>> GetEndpoints(Guid id, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToEndpointsQuery(id);
        var result = await _handler.GetSystemEndpointsAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToEndpointResponses(result));
    }

    [HttpGet("{id:guid}/capabilities")]
    public async Task<ActionResult<IReadOnlyList<SystemCapabilityResponse>>> GetCapabilities(Guid id, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToCapabilitiesQuery(id);
        var result = await _handler.GetSystemCapabilitiesAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToCapabilityResponses(result));
    }

    [HttpGet("by-deletion-status")]
    public async Task<ActionResult<IReadOnlyList<SystemResponse>>> GetByDeletionStatus([FromQuery] GetSystemsByDeletionStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(request);
        var result = await _handler.GetSystemsByDeletionStatusAsync(query, cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<IReadOnlyList<SystemResponse>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await _handler.GetDeletedSystemsAsync(cancellationToken);
        return Ok(SystemRegistryResponseMapper.ToResponse(result));
    }
}
