using Athena.Domain.Workload.Registry.Entities;

namespace Athena.Application.WorkloadRegistry.Contracts;

public static class WorkloadDtoMapper
{
    public static WorkloadDto ToDto(Workload workload)
    {
        return new WorkloadDto(
            workload.Id.Value,
            workload.BlockId.Value,
            workload.Name.Value,
            workload.RegistrationStatus.ToString(),
            workload.AvailabilityStatus.ToString(),
            workload.CreatedAtUtc,
            workload.UpdatedAtUtc,
            workload.IsDeleted);
    }
}