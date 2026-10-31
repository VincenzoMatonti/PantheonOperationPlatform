using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.Exceptions;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Block.Registry.Entities;

public class Block
{
    public BlockId Id { get; private set; } = null!;
    public EcosystemId EcosystemId { get; private set; } = null!;
    public BlockName Name { get; private set; } = null!;
    public BlockRegistrationStatus RegistrationStatus { get; private set; }
    public BlockAvailabilityStatus AvailabilityStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Block Create(BlockId id, EcosystemId ecosystemId, BlockName name)
    {
        if (id == null) throw new BlockIdRequiredException();
        if (ecosystemId == null) throw new BlockEcosystemIdRequiredException();
        if (name == null) throw new BlockNameRequiredException();
        return new Block
        {
            Id = id,
            EcosystemId = ecosystemId,
            Name = name,
            RegistrationStatus = BlockRegistrationStatus.Registered,
            AvailabilityStatus = BlockAvailabilityStatus.Unknown,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = null,
            IsDeleted = false
        };
    }

    // Identity management
    public void Rename(BlockName name)
    {
        EnsureRegistered();
        Name = name ?? throw new BlockNameRequiredException();
        Touch();
    }

    // Registration lifecycle
    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus != BlockRegistrationStatus.Unregistered) throw new BlockAlreadyRegisteredException(Name.Value);
        RegistrationStatus = BlockRegistrationStatus.Registered;
        AvailabilityStatus = BlockAvailabilityStatus.Unknown;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == BlockRegistrationStatus.Unregistered) throw new BlockAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = BlockRegistrationStatus.Unregistered;
        AvailabilityStatus = BlockAvailabilityStatus.Unknown;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == BlockRegistrationStatus.Unregistered) throw new BlockNotRegisteredException(Id.Value);
        if (RegistrationStatus == BlockRegistrationStatus.Registered) throw new BlockAlreadyEnabledException(Id.Value);
        RegistrationStatus = BlockRegistrationStatus.Registered;
        AvailabilityStatus = BlockAvailabilityStatus.Unknown;
        Touch();
    }

    public void Disable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == BlockRegistrationStatus.Unregistered) throw new BlockNotRegisteredException(Id.Value);
        if (RegistrationStatus == BlockRegistrationStatus.Disabled) throw new BlockAlreadyDisabledException(Id.Value);
        RegistrationStatus = BlockRegistrationStatus.Disabled;
        AvailabilityStatus = BlockAvailabilityStatus.Unknown;
        Touch();
    }

    // Availability lifecycle
    public void MarkAvailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == BlockAvailabilityStatus.Available) throw new BlockAlreadyAvailableException(Id.Value);
        AvailabilityStatus = BlockAvailabilityStatus.Available;
        Touch();
    }

    public void MarkUnavailable()
    {
        EnsureRegistered();
        if (AvailabilityStatus == BlockAvailabilityStatus.Unavailable) throw new BlockAlreadyUnavailableException(Id.Value);
        AvailabilityStatus = BlockAvailabilityStatus.Unavailable;
        Touch();
    }

    public void MarkAvailabilityUnknown()
    {
        EnsureRegistered();
        if (AvailabilityStatus == BlockAvailabilityStatus.Unknown) throw new BlockAlreadyAvailabilityUnknownException(Id.Value);
        AvailabilityStatus = BlockAvailabilityStatus.Unknown;
        Touch();
    }

    public void MarkError()
    {
        EnsureRegistered();
        if (AvailabilityStatus == BlockAvailabilityStatus.Error) throw new BlockAlreadyInErrorException(Id.Value);
        AvailabilityStatus = BlockAvailabilityStatus.Error;
        Touch();
    }

    public void Delete()
    {
        EnsureNotDeleted();
        IsDeleted = true;
        Touch();
    }

    private void EnsureRegistered()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == BlockRegistrationStatus.Unregistered) throw new BlockNotRegisteredException(Id.Value);
        if (RegistrationStatus == BlockRegistrationStatus.Disabled) throw new BlockAlreadyDisabledException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new BlockDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}