using FluentValidation;
using Hermes.Api.Endpoints.Requests;

namespace Hermes.Api.Endpoints.Validations;

public class GetEndpointOperationByIdRequestValidator : AbstractValidator<GetEndpointOperationByIdRequest>
{
    public GetEndpointOperationByIdRequestValidator()
    {
        RuleFor(x => x.EndpointOperationId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointOperationIdRequired)
            .WithMessage("Endpoint operation ID is required.");
    }
}

public class GetEndpointOperationRequestValidator : AbstractValidator<GetEndpointOperationRequest>
{
    public GetEndpointOperationRequestValidator()
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

public class GetEndpointOperationsByEndpointIdRequestValidator : AbstractValidator<GetEndpointOperationsByEndpointIdRequest>
{
    public GetEndpointOperationsByEndpointIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => EndpointOperationValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }
}

