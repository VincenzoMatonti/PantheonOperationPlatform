using Hermes.Application.Clients.Commands.ActivateClient;
using Hermes.Application.Clients.Commands.CreateClient;
using Hermes.Application.Clients.Commands.DeactivateClient;
using Hermes.Application.Clients.Commands.RenameClient;
using Hermes.Application.Clients.DTOs;
using Hermes.Application.Clients.Queries.GetClientByCode;
using Hermes.Application.Clients.Queries.GetClientById;
using Hermes.Application.Clients.Queries.GetClients;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientsController : ControllerBase
{
    private readonly CreateClientHandler _createClientHandler;
    private readonly ActivateClientHandler _activateClientHandler;
    private readonly DeactivateClientHandler _deactivateClientHandler;
    private readonly RenameClientHandler _renameClientHandler;

    private readonly GetClientByIdHandler _getClientByIdHandler;
    private readonly GetClientByCodeHandler _getClientByCodeHandler;
    private readonly GetClientsHandler _getClientsHandler;

    public ClientsController(
        CreateClientHandler createClientHandler,
        ActivateClientHandler activateClientHandler,
        DeactivateClientHandler deactivateClientHandler,
        RenameClientHandler renameClientHandler,
        GetClientByIdHandler getClientByIdHandler,
        GetClientByCodeHandler getClientByCodeHandler,
        GetClientsHandler getClientsHandler)
    {
        _createClientHandler = createClientHandler;
        _activateClientHandler = activateClientHandler;
        _deactivateClientHandler = deactivateClientHandler;
        _renameClientHandler = renameClientHandler;

        _getClientByIdHandler = getClientByIdHandler;
        _getClientByCodeHandler = getClientByCodeHandler;
        _getClientsHandler = getClientsHandler;
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var result = await _createClientHandler.HandleAsync(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getClientsHandler.HandleAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetClientByIdQuery
        {
            ClientId = id
        };

        var result = await _getClientByIdHandler.HandleAsync(query, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<ClientDto>> GetByCode(string code, CancellationToken cancellationToken)
    {
        var query = new GetClientByCodeQuery
        {
            Code = code
        };

        var result = await _getClientByCodeHandler.HandleAsync(query, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateClientCommand
        {
            ClientId = id
        };

        await _activateClientHandler.HandleAsync(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateClientCommand
        {
            ClientId = id
        };

        await _deactivateClientHandler.HandleAsync(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}/rename")]
    public async Task<IActionResult> Rename(Guid id, RenameClientCommand command, CancellationToken cancellationToken)
    {
        command.ClientId = id;
        await _renameClientHandler.HandleAsync(command, cancellationToken);

        return NoContent();
    }
}