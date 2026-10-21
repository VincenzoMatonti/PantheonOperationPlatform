using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.SystemRegistry.Mappings;
using Athena.Api.SystemRegistry.Requests;
using Athena.Api.SystemRegistry.Responses;
using Athena.Application.SystemRegistry.Enums;
using Athena.Application.SystemRegistry.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Athena.Api.SystemRegistry.Controllers;

[ApiController]
[Route("api/systems")]
public class SystemRegistryQueryController(SystemRegistryUseCaseHandler handler) : ControllerBase
{
    private readonly SystemRegistryUseCaseHandler _handler = handler;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> GetById(
        Guid id, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToByIdQuery(id);
        var result = await _handler.GetSystemByIdAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-name")]
    [ProducesResponseType(typeof(ApiResponse<SystemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemResponse>>> GetByName(
        [FromQuery] GetSystemByNameRequest request, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(request);
        var result = await _handler.GetSystemByNameAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _handler.GetAllSystemsAsync(cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-registration-status/{status}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemResponse>>>> GetByRegistrationStatus(
        SystemRegistrationStatusFilter status, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(status);
        var result = await _handler.GetSystemsByRegistrationStatusAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-availability-status/{status}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemResponse>>>> GetByAvailabilityStatus(
        SystemAvailabilityStatusFilter status, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(status);
        var result = await _handler.GetSystemsByAvailabilityStatusAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-capability")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemResponse>>>> GetByCapability(
        [FromQuery] GetSystemsByCapabilityRequest request, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(request);
        var result = await _handler.GetSystemsByCapabilityAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{id:guid}/endpoints")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemEndpointResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemEndpointResponse>>>> GetEndpoints(
        Guid id, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToEndpointsQuery(id);
        var result = await _handler.GetSystemEndpointsAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToEndpointResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("{id:guid}/capabilities")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemCapabilityResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemCapabilityResponse>>>> GetCapabilities(
        Guid id, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToCapabilitiesQuery(id);
        var result = await _handler.GetSystemCapabilitiesAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToCapabilityResponses(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("by-deletion-status")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemResponse>>>> GetByDeletionStatus(
        [FromQuery] GetSystemsByDeletionStatusRequest request, CancellationToken cancellationToken)
    {
        var query = SystemRegistryQueryRequestMapper.ToQuery(request);
        var result = await _handler.GetSystemsByDeletionStatusAsync(query, cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }

    [HttpGet("deleted")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SystemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SystemResponse>>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await _handler.GetDeletedSystemsAsync(cancellationToken);
        var response = SystemRegistryResponseMapper.ToResponse(result);
        return Ok(ApiResponseBuilder.BuildSuccess(response));
    }
}