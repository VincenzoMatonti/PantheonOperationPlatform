using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common.Responses;
using Hermes.Api.Clients.Requests;
using Hermes.Api.Clients.Responses;
using Hermes.Api.Clients.Mappings.Commands;
using Hermes.Application.Clients.UseCases;

namespace Hermes.Api.Clients.Controllers;

[ApiController]
[Route("api/client/command")]
public class ClientCommandApiController(
    ClientUseCaseHandler clientUseCase,
    ClientCommandResponseMapper responseMapper,
    ClientCommandRequestMapper requestMapper) : ControllerBase
{
    private readonly ClientUseCaseHandler _clientUseCase = clientUseCase;
    private readonly ClientCommandResponseMapper _responseMapper = responseMapper;
    private readonly ClientCommandRequestMapper _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateClientResponse>>> CreateClient(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.CreateClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateClientResponse>.Ok(response));
    }

    [HttpPut("rename")]
    public async Task<ActionResult<ApiResponse<RenameClientResponse>>> RenameClient(RenameClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.RenameClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RenameClientResponse>.Ok(response));
    }

    [HttpPut("renameCode")]
    public async Task<ActionResult<ApiResponse<RenameClientCodeResponse>>> RenameCode(RenameClientCodeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.RenameCodeClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RenameClientCodeResponse>.Ok(response));
    }

    [HttpPut("activate")]
    public async Task<ActionResult<ApiResponse<ActivateClientResponse>>> ActivateClient(ActivateClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.ActivateClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ActivateClientResponse>.Ok(response));
    }

    [HttpPut("deactivate")]
    public async Task<ActionResult<ApiResponse<DeactivateClientResponse>>> DeactivateClient(DeactivateClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.DeactivateClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeactivateClientResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteClientResponse>>> DeleteClient(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.DeletedClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteClientResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreClientResponse>>> RestoreClient(RestoreClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.RestoreClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreClientResponse>.Ok(response));
    }
}

