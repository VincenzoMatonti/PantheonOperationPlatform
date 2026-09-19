using FluentValidation;
using Hermes.Api.Endpoints.Requests;

namespace Hermes.Api.Endpoints.Validations;

public class CreateEndpointOperationRequestValidator : AbstractValidator<CreateEndpointOperationRequest>
{
    public CreateEndpointOperationRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");

        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.OperationTypeIdRequired)
            .WithMessage("Operation type ID is required.");
    }
}

public class EnableEndpointOperationRequestValidator : AbstractValidator<EnableEndpointOperationRequest>
{
    public EnableEndpointOperationRequestValidator()
    {
        RuleFor(x => x.EndpointOperationId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointOperationIdRequired)
            .WithMessage("Endpoint operation ID is required.");
    }
}

public class DisableEndpointOperationRequestValidator : AbstractValidator<DisableEndpointOperationRequest>
{
    public DisableEndpointOperationRequestValidator()
    {
        RuleFor(x => x.EndpointOperationId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointOperationIdRequired)
            .WithMessage("Endpoint operation ID is required.");
    }
}

public class DeleteEndpointOperationRequestValidator : AbstractValidator<DeleteEndpointOperationRequest>
{
    public DeleteEndpointOperationRequestValidator()
    {
        RuleFor(x => x.EndpointOperationId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointOperationIdRequired)
            .WithMessage("Endpoint operation ID is required.");
    }
}

public class RestoreEndpointOperationRequestValidator : AbstractValidator<RestoreEndpointOperationRequest>
{
    public RestoreEndpointOperationRequestValidator()
    {
        RuleFor(x => x.EndpointOperationId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointOperationIdRequired)
            .WithMessage("Endpoint operation ID is required.");
    }
}

