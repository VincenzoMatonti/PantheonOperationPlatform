using Athena.Api.SystemManagement.Requests;
using FluentValidation;

namespace Athena.Api.SystemManagement.Validators;

public sealed class CreateSystemInstanceRequestValidator : AbstractValidator<CreateSystemInstanceRequest>
{
    public CreateSystemInstanceRequestValidator()
    {
        RuleFor(request => request.SystemInstanceId).NotEmpty();
        RuleFor(request => request.SystemId).NotEmpty();
        RuleFor(request => request.WorkloadId).NotEmpty();
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.Details)
            .Must(string.IsNullOrWhiteSpace)
            .WithMessage("Inline details are not accepted for queued operations; use a configuration reference.");
    }
}

public sealed class RegisterSystemInstanceRequestValidator : AbstractValidator<RegisterSystemInstanceRequest>
{
    public RegisterSystemInstanceRequestValidator()
    {
        RuleFor(request => request.SystemInstanceId).NotEmpty();
        RuleFor(request => request.Details)
            .Must(string.IsNullOrWhiteSpace)
            .WithMessage("Inline details are not accepted for queued operations; use a configuration reference.");
    }
}

public sealed class ConfigureSystemInstanceRequestValidator : AbstractValidator<ConfigureSystemInstanceRequest>
{
    public ConfigureSystemInstanceRequestValidator() =>
        RuleFor(request => request.ConfigurationReference).NotEmpty();
}

public sealed class ResolveSystemInstanceOperationRequestValidator : AbstractValidator<ResolveSystemInstanceOperationRequest>
{
    public ResolveSystemInstanceOperationRequestValidator() =>
        RuleFor(request => request.Reason).NotEmpty();
}

public sealed class GetSystemInstanceLogsRequestValidator : AbstractValidator<GetSystemInstanceLogsRequest>
{
    public GetSystemInstanceLogsRequestValidator() =>
        RuleFor(request => request.Tail).InclusiveBetween(1, 1000);
}
