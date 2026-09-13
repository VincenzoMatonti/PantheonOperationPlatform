using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common.Responses;
using Hermes.Api.Clients.Requests;
using Hermes.Api.Clients.Responses;
using Hermes.Api.Clients.Mappings.Queries;
using Hermes.Application.Clients.UseCases;

namespace Hermes.Api.Clients.Controllers;

[ApiController]
[Route("api/clientOperation/query")]
public class ClientOperationQueryApiController(
    ClientOperationUseCaseHandler clientOperationUseCase,
    ClientOperationQueryResponseMapper responseMapper,
    ClientOperationQueryRequestMapper requestMapper) : ControllerBase
{
    private readonly ClientOperationUseCaseHandler _clientOperationUseCase = clientOperationUseCase;
    private readonly ClientOperationQueryResponseMapper _responseMapper = responseMapper;
    private readonly ClientOperationQueryRequestMapper _requestMapper = requestMapper;

    [HttpPost("byId")]
    public async Task<ActionResult<ApiResponse<ClientOperationResponse>>> GetClientOperationById(GetClientOperationByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientOperationUseCase.GetClientOperationByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ClientOperationResponse>.Ok(response));
    }

    [HttpPost("byClientAndOperationType")]
    public async Task<ActionResult<ApiResponse<ClientOperationResponse>>> GetClientOperation(GetClientOperationRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientOperationUseCase.GetClientOperationAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ClientOperationResponse>.Ok(response));
    }

    [HttpPost("byClientId")]
    public async Task<ActionResult<ApiResponse<List<ClientOperationResponse>>>> GetClientOperationsByClientId(GetClientOperationsByClientIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientOperationUseCase.GetClientOperationsByClientIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientOperationResponse>>.Ok(response));
    }

    [HttpPost("byOperationTypeId")]
    public async Task<ActionResult<ApiResponse<List<ClientOperationResponse>>>> GetClientOperationsByOperationTypeId(GetClientOperationsByOperationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientOperationUseCase.GetClientOperationsByOperationTypeIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientOperationResponse>>.Ok(response));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<List<ClientOperationResponse>>>> GetActiveClientOperations(CancellationToken cancellationToken)
    {
        var result = await _clientOperationUseCase.GetActiveClientOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientOperationResponse>>.Ok(response));
    }

    [HttpGet("nonActive")]
    public async Task<ActionResult<ApiResponse<List<ClientOperationResponse>>>> GetNonActiveClientOperations(CancellationToken cancellationToken)
    {
        var result = await _clientOperationUseCase.GetNonActiveClientOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientOperationResponse>>.Ok(response));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<ApiResponse<List<ClientOperationResponse>>>> GetDeletedClientOperations(CancellationToken cancellationToken)
    {
        var result = await _clientOperationUseCase.GetDeletedClientOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientOperationResponse>>.Ok(response));
    }
}