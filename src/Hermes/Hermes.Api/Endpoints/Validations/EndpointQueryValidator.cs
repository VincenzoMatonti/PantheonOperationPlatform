using FluentValidation;
using Hermes.Api.Endpoints.Requests;

namespace Hermes.Api.Endpoints.Validations;

public class GetEndpointByIdRequestValidator : AbstractValidator<GetEndpointByIdRequest>
{
    public GetEndpointByIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

public class GetEndpointByCodeRequestValidator : AbstractValidator<GetEndpointByCodeRequest>
{
    public GetEndpointByCodeRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointCodeRequired)
            .WithMessage("Endpoint code is required.")
            .MaximumLength(100)
            .WithState(_ => EndpointValidationErrorCode.EndpointCodeMaximumLength)
            .WithMessage("Endpoint code cannot exceed 100 characters.");
    }
}

public class GetEndpointByTypeRequestValidator : AbstractValidator<GetEndpointByTypeRequest>
{
    public GetEndpointByTypeRequestValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointTypeRequired)
            .WithMessage("Endpoint type is required.");
    }
}

public class GetEndpointCodeByIdRequestValidator : AbstractValidator<GetEndpointCodeByIdRequest>
{
    public GetEndpointCodeByIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

