using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Clients.Requests;
using Hermes.Api.Clients.Responses;
using Hermes.Api.Clients.Mappings.Commands;
using Hermes.Application.Clients.UseCases;

namespace Hermes.Api.Clients.Controllers;

[ApiController]
[Route("api/clientOperation/command")]
public class ClientOperationCommandApiController(
    ClientOperationUseCaseHandler clientOperationUseCase,
    ClientOperationCommandResponseMapper responseMapper,
    ClientOperationCommandRequestMapper requestMapper) : ControllerBase
{
    private readonly ClientOperationUseCaseHandler _clientOperationUseCase = clientOperationUseCase;
    private readonly ClientOperationCommandResponseMapper _responseMapper = responseMapper;
    private readonly ClientOperationCommandRequestMapper _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateClientOperationResponse>>> CreateClientOperation(CreateClientOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientOperationUseCase.CreateClientOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateClientOperationResponse>.Ok(response));
    }

    [HttpPut("enable")]
    public async Task<ActionResult<ApiResponse<EnableClientOperationResponse>>> EnableClientOperation(EnableClientOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientOperationUseCase.EnableClientOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EnableClientOperationResponse>.Ok(response));
    }

    [HttpPut("disable")]
    public async Task<ActionResult<ApiResponse<DisableClientOperationResponse>>> DisableClientOperation(DisableClientOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientOperationUseCase.DisableClientOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DisableClientOperationResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteClientOperationResponse>>> DeleteClientOperation(DeleteClientOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientOperationUseCase.DeleteClientOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteClientOperationResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreClientOperationResponse>>> RestoreClientOperation(RestoreClientOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientOperationUseCase.RestoreClientOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreClientOperationResponse>.Ok(response));
    }
}