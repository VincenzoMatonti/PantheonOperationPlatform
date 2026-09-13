using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Clients.Requests;
using Hermes.Api.Clients.Responses;
using Hermes.Api.Clients.Mappings.Queries;
using Hermes.Application.Clients.UseCases;

namespace Hermes.Api.Clients.Controllers;

[ApiController]
[Route("api/client/query")]
public class ClientQueryApiController(
    ClientUseCaseHandler clientUseCase,
    ClientQueryResponseMapper responseMapper,
    ClientQueryRequestMapper requestMapper) : ControllerBase
{
    private readonly ClientUseCaseHandler _clientUseCase = clientUseCase;
    private readonly ClientQueryResponseMapper _responseMapper = responseMapper;
    private readonly ClientQueryRequestMapper _requestMapper = requestMapper;

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<ClientResponse>>>> GetAllClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCase.GetAllClientsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientResponse>>.Ok(response));
    }

    [HttpPost("byId")]
    public async Task<ActionResult<ApiResponse<ClientResponse>>> GetClientById(GetClientByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientUseCase.GetClientByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ClientResponse>.Ok(response));
    }

    [HttpPost("byCode")]
    public async Task<ActionResult<ApiResponse<ClientResponse>>> GetClientByCode(GetClientByCodeRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientUseCase.GetClientByCodeAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ClientResponse>.Ok(response));
    }

    [HttpPost("codeById")]
    public async Task<ActionResult<ApiResponse<ClientCodeResponse>>> GetClientCodeById(GetClientCodeByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _clientUseCase.GetClientCodeByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ClientCodeResponse>.Ok(response));
    }

    [HttpGet("allCodes")]
    public async Task<ActionResult<ApiResponse<List<ClientCodeResponse>>>> GetAllClientCodes(CancellationToken cancellationToken)
    {
        var result = await _clientUseCase.GetAllClientCodesAsync(cancellationToken);
        var response = _responseMapper.ToCodeResponse(result);
        return Ok(ApiResponse<List<ClientCodeResponse>>.Ok(response));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<List<ClientResponse>>>> GetActiveClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCase.GetActiveClientsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientResponse>>.Ok(response));
    }

    [HttpGet("nonActive")]
    public async Task<ActionResult<ApiResponse<List<ClientResponse>>>> GetNonActiveClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCase.GetNonActiveClientsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientResponse>>.Ok(response));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<ApiResponse<List<ClientResponse>>>> GetDeletedClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCase.GetDeletedClientsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<ClientResponse>>.Ok(response));
    }
}

