using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.Repositories;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Application.Clients.DTOs;

namespace Hermes.Application.Clients.Commands.CreateClient;

public class CreateClientHandler
{
    private readonly IClientRepository _clientRepository;

    public CreateClientHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto> HandleAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
    {
        var code = ClientCode.Create(command.Code);

        var existingClient = await _clientRepository.GetByCodeAsync(code, cancellationToken);

        if (existingClient is not null)
        {
            throw new InvalidOperationException($"A client with code '{code.Value}' already exists.");
        }

        var client = Client.Create(code, command.Name);

        await _clientRepository.AddAsync(client, cancellationToken);

        return new ClientDto
        {
            Id = client.Id,
            Code = client.Code.Value,
            Name = client.Name,
            IsActive = client.IsActive
        };
    }
}