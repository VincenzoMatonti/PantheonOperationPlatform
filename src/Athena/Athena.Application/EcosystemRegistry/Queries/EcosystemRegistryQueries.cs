using Athena.Application.EcosystemRegistry.Enums;

namespace Athena.Application.EcosystemRegistry.Queries;

public class GetEcosystemByIdQuery
{
    public Guid EcosystemId { get; set; }
}

public class GetEcosystemByNameQuery
{
    public required string Name { get; set; }
}

public class GetEcosystemsByCompanyIdQuery
{
    public Guid CompanyId { get; set; }
}

public class GetEcosystemsByRegistrationStatusQuery
{
    public EcosystemRegistrationStatusFilter Status { get; set; }
}

public class GetEcosystemsByAvailabilityStatusQuery
{
    public EcosystemAvailabilityStatusFilter Status { get; set; }
}

public class GetEcosystemsByDeletionStatusQuery
{
    public bool IsDeleted { get; set; }
}