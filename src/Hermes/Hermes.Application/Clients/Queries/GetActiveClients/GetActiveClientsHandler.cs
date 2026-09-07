using Hermes.Application.Clients.DTOs;
using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Queries.GetActiveClients;

public class GetActiveClientsHandler
{
    private readonly IClientRepository _clientRepository;

    public GetActiveClientsHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<List<ActiveClientDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var clients = await _clientRepository.GetActiveClientAsync(cancellationToken);
        var activeClients = clients.Select(client => new ActiveClientDto
        {
            Id = client.Id,
            Code = client.Code.Value,
            Name = client.Name
        })
        .ToList();

        return activeClients;

    }
}