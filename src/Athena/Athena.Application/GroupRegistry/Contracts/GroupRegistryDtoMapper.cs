using Athena.Domain.Company.Entities;

namespace Athena.Application.GroupRegistry.Contracts;

public static class GroupRegistryDtoMapper
{
    public static GroupDto ToDto(Group group) => new()
    {
        Id = group.Id.Value,
        Name = group.Name.Value,
        RegistrationStatus = group.RegistrationStatus.ToString(),
        CreatedAtUtc = group.CreatedAtUtc,
        UpdatedAtUtc = group.UpdatedAtUtc,
        IsDeleted = group.IsDeleted
    };

    public static List<GroupDto> ToDtos(IEnumerable<Group> groups) => [.. groups.Select(ToDto)];
}