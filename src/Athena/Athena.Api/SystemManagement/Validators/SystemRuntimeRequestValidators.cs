using Athena.Api.SystemManagement.Requests;
using FluentValidation;

namespace Athena.Api.SystemManagement.Validators;

public class GetSystemRuntimeInstanceByIdRequestValidator
    : AbstractValidator<GetSystemRuntimeInstanceByIdRequest>
{
    public GetSystemRuntimeInstanceByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetSystemRuntimeInstancesBySystemIdRequestValidator
    : AbstractValidator<GetSystemRuntimeInstancesBySystemIdRequest>
{
    public GetSystemRuntimeInstancesBySystemIdRequestValidator()
    {
        RuleFor(x => x.SystemId).NotEmpty();
    }
}

public class GetSystemRuntimeInstancesBySystemIdAndEnvironmentRequestValidator
    : AbstractValidator<GetSystemRuntimeInstancesBySystemIdAndEnvironmentRequest>
{
    public GetSystemRuntimeInstancesBySystemIdAndEnvironmentRequestValidator()
    {
        RuleFor(x => x.SystemId).NotEmpty();
        RuleFor(x => x.Environment).NotEmpty().MaximumLength(100);
    }
}

public class GetSystemRuntimeInstancesByRuntimeResourceNameRequestValidator
    : AbstractValidator<GetSystemRuntimeInstancesByRuntimeResourceNameRequest>
{
    public GetSystemRuntimeInstancesByRuntimeResourceNameRequestValidator()
    {
        RuleFor(x => x.RuntimeResourceName).NotEmpty().MaximumLength(255);
    }
}

public class RegisterSystemRuntimeInstanceRequestValidator : AbstractValidator<RegisterSystemRuntimeInstanceRequest>
{
    public RegisterSystemRuntimeInstanceRequestValidator()
    {
        RuleFor(x => x.SystemId).NotEmpty();
        RuleFor(x => x.RuntimeProvider).NotEmpty().MaximumLength(100);
        RuleFor(x => x.RuntimeResourceId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.RuntimeResourceName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Environment).NotEmpty().MaximumLength(100);
    }
}

public class UnregisterSystemRuntimeInstanceRequestValidator : AbstractValidator<UnregisterSystemRuntimeInstanceRequest>
{
    public UnregisterSystemRuntimeInstanceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
