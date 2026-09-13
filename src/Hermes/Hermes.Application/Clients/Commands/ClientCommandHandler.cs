using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Domain.Clients.Repositories.ClientRepositories;

namespace Hermes.Application.Clients.Commands
{
    public class ClientCommandHandler(IClientCommandRepository clientCommandRepository)
    {
        private readonly IClientCommandRepository _clientCommandRepository = clientCommandRepository;

        public async Task<Client> CreateClientAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var code = ClientCode.Create(command.Code);
            var client = Client.Create(code, command.Name);
            await _clientCommandRepository.AddAsync(client, cancellationToken);
            return client;
        }

        public async Task RenameClientAsync(Client client, string newName)
        {
            client.Rename(newName);
            _clientCommandRepository.Update(client);
        }

        public async Task RenameCodeClientAsync(Client client, string newCode)
        {
            var code = ClientCode.Create(newCode);
            client.RenameCode(code);
            _clientCommandRepository.Update(client);
        }

        public async Task ActivateClientAsync(Client client)
        {
            client.Activate();
            _clientCommandRepository.Update(client);
        }

        public async Task DeactivateClientAsync(Client client)
        {
            client.Deactivate();
            _clientCommandRepository.Update(client);
        }

        public async Task DeleteClientAsync(Client client)
        {
            client.MarkAsDeleted();
            _clientCommandRepository.Update(client);
        }

        public async Task RestoreClientAsync(Client client)
        {
            client.Restore();
            _clientCommandRepository.Update(client);
        }

        public static CreateClientDto ConvertClientEntitesToCreateDto(Client client)
        {
            return new CreateClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                CreatedAt = client.CreatedAt
            };
        }

        public static List<CreateClientDto> ConvertClientEntitesToCreateDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToCreateDto)];
        }

        public static RenameClientDto ConvertClientEntitesToRenameDto(Client client)
        {
            return new RenameClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                UpdatedAt = client.UpdatedAt
            };
        }

         public static List<RenameClientDto> ConvertClientEntitesToRenameDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToRenameDto)];
        }

        public static RenameClientCodeDto ConvertClientEntitesToRenameCodeDto(Client client)
        {
            return new RenameClientCodeDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                UpdatedAt = client.UpdatedAt
            };
        }

         public static List<RenameClientCodeDto> ConvertClientEntitesToRenameCodeDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToRenameCodeDto)];
        }

        public static ActivateClientDto ConvertClientEntitesToActivateDto(Client client)
        {
            return new ActivateClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                IsActive = client.IsActive,
                UpdatedAt = client.UpdatedAt
            };
        }

         public static List<ActivateClientDto> ConvertClientEntitesToActivateDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToActivateDto)];
        }

        public static DeactivateClientDto ConvertClientEntitesToDeactivateDto(Client client)
        {
            return new DeactivateClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                IsActive = client.IsActive,
                UpdatedAt = client.UpdatedAt
            };
        }

         public static List<DeactivateClientDto> ConvertClientEntitesToDeactivateDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToDeactivateDto)];
        }

        public static DeleteClientDto ConvertClientEntitesToDeleteDto(Client client)
        {
            return new DeleteClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                IsDeleted = client.IsDeleted,
                UpdatedAt = client.UpdatedAt
            };
        }

         public static List<DeleteClientDto> ConvertClientEntitesToDeleteDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToDeleteDto)];
        }

        public static RestoreClientDto ConvertClientEntitesToRestoreDto(Client client)
        {
            return new RestoreClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                IsDeleted = client.IsDeleted,
                UpdatedAt = client.UpdatedAt
            };
        }

         public static List<RestoreClientDto> ConvertClientEntitesToRestoreDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToRestoreDto)];
        }
    }
}
