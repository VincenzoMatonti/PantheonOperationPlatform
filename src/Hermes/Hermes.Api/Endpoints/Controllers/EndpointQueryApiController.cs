using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Endpoints.Requests;
using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.UseCases;
using Hermes.Api.Endpoints.Mappings.Queries;

namespace Hermes.Api.Endpoints.Controllers;

[ApiController]
[Route("api/endpoint/query")]
public class EndpointQueryApiController(
    EndpointUseCaseHandler endpointUseCase,
    EndpointQueryResponseMapper responseMapper,
    EndpointQueryRequestMapper requestMapper) : ControllerBase
{
    private readonly EndpointUseCaseHandler _endpointUseCase = endpointUseCase;
    private readonly EndpointQueryResponseMapper _responseMapper = responseMapper;
    private readonly EndpointQueryRequestMapper _requestMapper = requestMapper;

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<EndpointResponse>>>> GetAllEndpoints(CancellationToken cancellationToken)
    {
        var result = await _endpointUseCase.GetAllEndpointsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointResponse>>.Ok(response));
    }

    [HttpPost("byId")]
    public async Task<ActionResult<ApiResponse<EndpointResponse>>> GetEndpointById(GetEndpointByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointUseCase.GetEndpointByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EndpointResponse>.Ok(response));
    }

    [HttpPost("byCode")]
    public async Task<ActionResult<ApiResponse<EndpointResponse>>> GetEndpointByCode(GetEndpointByCodeRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointUseCase.GetEndpointByCodeAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EndpointResponse>.Ok(response));
    }

    [HttpPost("byType")]
    public async Task<ActionResult<ApiResponse<EndpointResponse>>> GetEndpointByType(GetEndpointByTypeRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointUseCase.GetEndpointByTypeAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EndpointResponse>.Ok(response));
    }

    [HttpPost("codeById")]
    public async Task<ActionResult<ApiResponse<EndpointCodeResponse>>> GetEndpointCodeById(GetEndpointCodeByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointUseCase.GetEndpointCodeByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EndpointCodeResponse>.Ok(response));
    }

    [HttpGet("allCodes")]
    public async Task<ActionResult<ApiResponse<List<EndpointCodeResponse>>>> GetAllEndpointCodes(CancellationToken cancellationToken)
    {
        var result = await _endpointUseCase.GetAllEndpointCodesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointCodeResponse>>.Ok(response));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<List<EndpointResponse>>>> GetActiveEndpoints(CancellationToken cancellationToken)
    {
        var result = await _endpointUseCase.GetActiveEndpointsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointResponse>>.Ok(response));
    }

    [HttpGet("nonActive")]
    public async Task<ActionResult<ApiResponse<List<EndpointResponse>>>> GetNonActiveEndpoints(CancellationToken cancellationToken)
    {
        var result = await _endpointUseCase.GetNonActiveEndpointsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointResponse>>.Ok(response));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<ApiResponse<List<EndpointResponse>>>> GetDeletedEndpoints(CancellationToken cancellationToken)
    {
        var result = await _endpointUseCase.GetDeletedEndpointsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointResponse>>.Ok(response));
    }
}

