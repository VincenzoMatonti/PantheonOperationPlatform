using Athena.Application.GroupRegistry.Enums;

namespace Athena.Application.GroupRegistry.Queries;

public class GetGroupByIdQuery
{
    public Guid GroupId { get; set; }
}

public class GetGroupByNameQuery
{
    public required string Name { get; set; }
}

public class GetGroupsByRegistrationStatusQuery
{
    public GroupRegistrationStatusFilter Status { get; set; }
}

public class GetGroupsByDeletionStatusQuery
{
    public bool IsDeleted { get; set; }
}
