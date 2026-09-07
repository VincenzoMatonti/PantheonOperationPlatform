using Hermes.Application.Clients.DTOs;
using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Queries.GetClientById;

public class GetClientByIdHandler
{
    private readonly IClientRepository _clientRepository;

    public GetClientByIdHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto?> HandleAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(query.ClientId, cancellationToken);

        if (client is null)
        {
            return null;
        }

        return new ClientDto
        {
            Id = client.Id,
            Code = client.Code.Value,
            Name = client.Name,
            IsActive = client.IsActive
        };
    }
}