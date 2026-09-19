using FluentValidation;
using Hermes.Api.Endpoints.Requests;

namespace Hermes.Api.Endpoints.Validations;

public class CreateEndpointRequestValidator : AbstractValidator<CreateEndpointRequest>
{
    public CreateEndpointRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointCodeRequired)
            .WithMessage("Endpoint code is required.")
            .MaximumLength(100)
            .WithState(_ => EndpointValidationErrorCode.EndpointCodeMaximumLength)
            .WithMessage("Endpoint code cannot exceed 100 characters.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointTypeRequired)
            .WithMessage("Endpoint type is required.");
    }
}

public class ChangeEndpointTypeRequestValidator : AbstractValidator<ChangeEndpointTypeRequest>
{
    public ChangeEndpointTypeRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");

        RuleFor(x => x.NewType)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointTypeRequired)
            .WithMessage("New endpoint type is required.");
    }
}

public class RenameCodeEndpointRequestValidator : AbstractValidator<RenameCodeEndpointRequest>
{
    public RenameCodeEndpointRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");

        RuleFor(x => x.NewCode)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointCodeRequired)
            .WithMessage("New endpoint code is required.")
            .MaximumLength(100)
            .WithState(_ => EndpointValidationErrorCode.EndpointCodeMaximumLength)
            .WithMessage("Endpoint code cannot exceed 100 characters.");
    }
}

public class ActivateEndpointRequestValidator : AbstractValidator<ActivateEndpointRequest>
{
    public ActivateEndpointRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

public class DeactivateEndpointRequestValidator : AbstractValidator<DeactivateEndpointRequest>
{
    public DeactivateEndpointRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

public class DeleteEndpointRequestValidator : AbstractValidator<DeleteEndpointRequest>
{
    public DeleteEndpointRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

public class RestoreEndpointRequestValidator : AbstractValidator<RestoreEndpointRequest>
{
    public RestoreEndpointRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

