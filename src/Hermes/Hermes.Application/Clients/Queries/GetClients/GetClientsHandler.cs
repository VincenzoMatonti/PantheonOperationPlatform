using Hermes.Application.Clients.DTOs;
using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Queries.GetClients;

public class GetClientsHandler
{
    private readonly IClientRepository _clientRepository;

    public GetClientsHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<List<ClientDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var clients = await _clientRepository.GetAllAsync(cancellationToken);

        return clients.Select(client => new ClientDto
        {
            Id = client.Id,
            Code = client.Code.Value,
            Name = client.Name,
            IsActive = client.IsActive
        })
        .ToList();

    }
}