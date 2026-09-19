using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Endpoints.Requests;
using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.UseCases;
using Hermes.Api.Endpoints.Mappings.Queries;

namespace Hermes.Api.Endpoints.Controllers;

[ApiController]
[Route("api/endpointOperation/query")]
public class EndpointOperationQueryApiController(
    EndpointOperationUseCaseHandler endpointOperationUseCase,
    EndpointOperationQueryResponseMapping responseMapper,
    EndpointOperationQueryRequestMapping requestMapper) : ControllerBase
{
    private readonly EndpointOperationUseCaseHandler _endpointOperationUseCase = endpointOperationUseCase;
    private readonly EndpointOperationQueryResponseMapping _responseMapper = responseMapper;
    private readonly EndpointOperationQueryRequestMapping _requestMapper = requestMapper;

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<EndpointOperationResponse>>>> GetAllEndpointOperations(CancellationToken cancellationToken)
    {
        var result = await _endpointOperationUseCase.GetAllEndpointOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointOperationResponse>>.Ok(response));
    }

    [HttpPost("byId")]
    public async Task<ActionResult<ApiResponse<EndpointOperationResponse>>> GetEndpointOperationById(GetEndpointOperationByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointOperationUseCase.GetEndpointOperationByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EndpointOperationResponse>.Ok(response));
    }

    [HttpPost("byEndpointAndOperationType")]
    public async Task<ActionResult<ApiResponse<EndpointOperationResponse>>> GetEndpointOperation(GetEndpointOperationRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointOperationUseCase.GetEndpointOperationAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EndpointOperationResponse>.Ok(response));
    }

    [HttpPost("byEndpointId")]
    public async Task<ActionResult<ApiResponse<List<EndpointOperationResponse>>>> GetEndpointOperationsByEndpointId(GetEndpointOperationsByEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _endpointOperationUseCase.GetEndpointOperationsByEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointOperationResponse>>.Ok(response));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<List<EndpointOperationResponse>>>> GetActiveEndpointOperations(CancellationToken cancellationToken)
    {
        var result = await _endpointOperationUseCase.GetActiveEndpointOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointOperationResponse>>.Ok(response));
    }

    [HttpGet("nonActive")]
    public async Task<ActionResult<ApiResponse<List<EndpointOperationResponse>>>> GetNonActiveEndpointOperations(CancellationToken cancellationToken)
    {
        var result = await _endpointOperationUseCase.GetNonActiveEndpointOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointOperationResponse>>.Ok(response));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<ApiResponse<List<EndpointOperationResponse>>>> GetDeletedEndpointOperations(CancellationToken cancellationToken)
    {
        var result = await _endpointOperationUseCase.GetDeletedEndpointOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<EndpointOperationResponse>>.Ok(response));
    }
}

