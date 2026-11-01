using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Management.Enums;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Management.Enums;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.UnitTests;

public sealed class ParentLifecycleDesiredStateTests
{
    [Fact]
    public void WorkloadDesiredState_DoesNotAssertOperationalStatus()
    {
        var lifecycle = WorkloadLifecycle.Create(WorkloadId.Create(Guid.NewGuid()));

        lifecycle.SetDesiredState(WorkloadDesiredState.Running);

        Assert.Equal(WorkloadOperationalStatus.Unknown, lifecycle.OperationalStatus);
    }

    [Fact]
    public void BlockDesiredState_DoesNotAssertOperationalStatus()
    {
        var lifecycle = BlockLifecycle.Create(BlockId.Create(Guid.NewGuid()));

        lifecycle.SetDesiredState(BlockDesiredState.Running);

        Assert.Equal(BlockOperationalStatus.Unknown, lifecycle.OperationalStatus);
    }

    [Fact]
    public void EcosystemDesiredState_DoesNotAssertOperationalStatus()
    {
        var lifecycle = EcosystemLifecycle.Create(EcosystemId.Create(Guid.NewGuid()));

        lifecycle.SetDesiredState(EcosystemDesiredState.Running);

        Assert.Equal(EcosystemOperationalStatus.Unknown, lifecycle.OperationalStatus);
    }
}
