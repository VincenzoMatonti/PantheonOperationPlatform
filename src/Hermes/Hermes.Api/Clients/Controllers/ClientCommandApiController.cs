using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<CreateClientResponse>> CreateClient(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _clientUseCase.CreateClientAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(response);
    }

    [HttpPut("rename")]
    public async Task<IActionResult> RenameClient(RenameClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        await _clientUseCase.RenameClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("renameCode")]
    public async Task<IActionResult> RenameCode(RenameClientCodeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        await _clientUseCase.RenameCodeClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("activate")]
    public async Task<IActionResult> ActivateClient(ActivateClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        await _clientUseCase.ActivateClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("deactivate")]
    public async Task<IActionResult> DeactivateClient(DeactivateClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        await _clientUseCase.DeactivateClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteClient(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        await _clientUseCase.DeletedClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("restore")]
    public async Task<IActionResult> RestoreClient(RestoreClientRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        await _clientUseCase.RestoreClientAsync(command, cancellationToken);
        return NoContent();
    }
}

