using Athena.Application.BlockRegistry.Enums;

namespace Athena.Application.BlockRegistry.Queries;

public class GetBlockByIdQuery
{
    public Guid BlockId { get; set; }
}

public class GetBlockByNameQuery
{
    public required string Name { get; set; }
}

public class GetBlocksByEcosystemIdQuery
{
    public Guid EcosystemId { get; set; }
}

public class GetBlocksByRegistrationStatusQuery
{
    public BlockRegistrationStatusFilter Status { get; set; }
}

public class GetBlocksByAvailabilityStatusQuery
{
    public BlockAvailabilityStatusFilter Status { get; set; }
}

public class GetBlocksByDeletionStatusQuery
{
    public bool IsDeleted { get; set; }
}