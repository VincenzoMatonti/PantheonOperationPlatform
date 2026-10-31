using Athena.Domain.Company.Registry.Enums;
using Athena.Domain.Company.Registry.Exceptions;
using Athena.Domain.Company.Registry.ValueObjects;

namespace Athena.Domain.Company.Registry.Entities;

public class Company
{
    public CompanyId Id { get; private set; } = null!;
    public GroupId? GroupId { get; private set; }
    public CompanyName Name { get; private set; } = null!;
    public LegalName LegalName { get; private set; } = null!;
    public CountryCode CountryCode { get; private set; } = null!;
    public TaxCode? TaxCode { get; private set; }
    public VatNumber? VatNumber { get; private set; }
    public RegistrationNumber? RegistrationNumber { get; private set; }
    public LegalForm? LegalForm { get; private set; }
    public EmailAddress? Email { get; private set; }
    public PecAddress? Pec { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public WebsiteUrl? Website { get; private set; }
    public PostalAddress? RegisteredAddress { get; private set; }
    public CompanyRegistrationStatus RegistrationStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Company Create(
        CompanyId id,
        CompanyName name,
        LegalName legalName,
        CountryCode countryCode,
        GroupId? groupId = null,
        TaxCode? taxCode = null,
        VatNumber? vatNumber = null,
        RegistrationNumber? registrationNumber = null,
        LegalForm? legalForm = null,
        EmailAddress? email = null,
        PecAddress? pec = null,
        PhoneNumber? phoneNumber = null,
        WebsiteUrl? website = null,
        PostalAddress? registeredAddress = null)
    {
        if (id == null) throw new CompanyIdRequiredException();
        if (name == null) throw new CompanyNameRequiredException();
        if (legalName == null) throw new LegalNameRequiredException();
        if (countryCode == null) throw new CountryCodeRequiredException();
        return new Company
        {
            Id = id,
            GroupId = groupId,
            Name = name,
            LegalName = legalName,
            CountryCode = countryCode,
            TaxCode = taxCode,
            VatNumber = vatNumber,
            RegistrationNumber = registrationNumber,
            LegalForm = legalForm,
            Email = email,
            Pec = pec,
            PhoneNumber = phoneNumber,
            Website = website,
            RegisteredAddress = registeredAddress,
            RegistrationStatus = CompanyRegistrationStatus.Registered,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = null,
            IsDeleted = false
        };
    }

    // Group management
    public void AssignToGroup(GroupId groupId)
    {
        EnsureRegistered();
        GroupId = groupId ?? throw new GroupIdRequiredException();
        Touch();
    }

    public void RemoveFromGroup()
    {
        EnsureRegistered();
        if (GroupId == null) return;
        GroupId = null;
        Touch();
    }

    // Identity management
    public void Rename(CompanyName name)
    {
        EnsureRegistered();
        Name = name ?? throw new CompanyNameRequiredException();
        Touch();
    }

    public void ChangeLegalName(LegalName legalName)
    {
        EnsureRegistered();
        LegalName = legalName ?? throw new LegalNameRequiredException();
        Touch();
    }

    // Fiscal and legal information
    public void UpdateFiscalInformation(CountryCode countryCode, TaxCode? taxCode, VatNumber? vatNumber, RegistrationNumber? registrationNumber, LegalForm? legalForm)
    {
        EnsureRegistered();
        CountryCode = countryCode ?? throw new CountryCodeRequiredException();
        TaxCode = taxCode;
        VatNumber = vatNumber;
        RegistrationNumber = registrationNumber;
        LegalForm = legalForm;
        Touch();
    }

    // Contact information
    public void UpdateContactInformation(EmailAddress? email, PecAddress? pec, PhoneNumber? phoneNumber, WebsiteUrl? website)
    {
        EnsureRegistered();
        Email = email;
        Pec = pec;
        PhoneNumber = phoneNumber;
        Website = website;
        Touch();
    }

    // Registered address
    public void UpdateRegisteredAddress(PostalAddress? registeredAddress)
    {
        EnsureRegistered();
        RegisteredAddress = registeredAddress;
        Touch();
    }

    // Registration lifecycle
    public void Register()
    {
        EnsureNotDeleted();
        if (RegistrationStatus != CompanyRegistrationStatus.Unregistered) throw new CompanyAlreadyRegisteredException(Name.Value);
        RegistrationStatus = CompanyRegistrationStatus.Registered;
        Touch();
    }

    public void Unregister()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == CompanyRegistrationStatus.Unregistered) throw new CompanyAlreadyUnregisteredException(Id.Value);
        RegistrationStatus = CompanyRegistrationStatus.Unregistered;
        Touch();
    }

    public void Enable()
    {
        EnsureNotDeleted();
        if (RegistrationStatus == CompanyRegistrationStatus.Unregistered) throw new CompanyNotRegisteredException(Id.Value);
        if (RegistrationStatus == CompanyRegistrationStatus.Registered) throw new CompanyAlreadyEnabledException(Id.Value);
        RegistrationStatus = CompanyRegistrationStatus.Registered;
        Touch();
    }

    public void Disable()
    {
        EnsureRegistered();
        if (RegistrationStatus == CompanyRegistrationStatus.Disabled) throw new CompanyAlreadyDisabledException(Id.Value);
        RegistrationStatus = CompanyRegistrationStatus.Disabled;
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
        if (RegistrationStatus == CompanyRegistrationStatus.Unregistered) throw new CompanyNotRegisteredException(Id.Value);
        if (RegistrationStatus == CompanyRegistrationStatus.Disabled) throw new CompanyAlreadyDisabledException(Id.Value);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted) throw new CompanyDeletedCannotBeModifiedException(Id.Value);
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}