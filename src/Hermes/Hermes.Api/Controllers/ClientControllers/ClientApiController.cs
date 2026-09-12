using Microsoft.AspNetCore.Mvc;
using Hermes.Application.Clients.UseCases;
using Hermes.Application.Clients.DTOs.ClientDTOs;

namespace Hermes.Api.Controllers.ClientControllers;

[ApiController]
[Route("api/client")]
public class ClientApiController(ClientUseCaseHandler clientUseCaseHandler) : ControllerBase
{
    private readonly ClientUseCaseHandler _clientUseCaseHandler = clientUseCaseHandler;

    [HttpPost("/create")]
    public async Task<ActionResult<ClientDto>> CreateClient(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.CreateClientAsync(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("/rename")]
    public async Task<IActionResult> RenameClient(RenameClientCommand command, CancellationToken cancellationToken)
    {
        await _clientUseCaseHandler.RenameClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("/renameCode")]
    public async Task<IActionResult> RenameCode(RenameCodeClientCommand command, CancellationToken cancellationToken)
    {
        await _clientUseCaseHandler.RenameCodeClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("/activate")]
    public async Task<IActionResult> ActivateClient(ActivateClientCommand command, CancellationToken cancellationToken)
    {
        await _clientUseCaseHandler.ActivateClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("/deactivate")]
    public async Task<IActionResult> DeactivateClient(DeactivateClientCommand command, CancellationToken cancellationToken)
    {
        await _clientUseCaseHandler.DeactivateClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("/delete")]
    public async Task<IActionResult> DeleteClient(DeletedClientCommand command, CancellationToken cancellationToken)
    {
        await _clientUseCaseHandler.DeletedClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("/restore")]
    public async Task<IActionResult> RestoreClient(RestoreClientCommand command, CancellationToken cancellationToken)
    {
        await _clientUseCaseHandler.RestoreClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("/all")]
    public async Task<ActionResult<List<ClientDto>>> GetAllClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetAllClientsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("/byId")]
    public async Task<ActionResult<ClientDto>> GetClientById(GetClientByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetClientByIdAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("/byCode")]
    public async Task<ActionResult<ClientDto>> GetClientByCode(GetClientByCodeQuery query, CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetClientByCodeAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("/codeById")]
    public async Task<ActionResult<ClientCodeDto>> GetClientCodeById(GetClientCodeByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetClientCodeByIdAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("/allCodes")]
    public async Task<ActionResult<List<ClientCodeDto>>> GetAllClientCodes(CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetAllClientCodesAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("/active")]
    public async Task<ActionResult<List<ClientDto>>> GetActiveClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetActiveClientsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("/nonActive")]
    public async Task<ActionResult<List<ClientDto>>> GetNonActiveClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetNonActiveClientsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("/deleted")]
    public async Task<ActionResult<List<ClientDto>>> GetDeletedClients(CancellationToken cancellationToken)
    {
        var result = await _clientUseCaseHandler.GetDeletedClientsAsync(cancellationToken);
        return Ok(result);
    }
}