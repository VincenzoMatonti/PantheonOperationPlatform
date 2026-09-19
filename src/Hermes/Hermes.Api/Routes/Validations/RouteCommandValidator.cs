using FluentValidation;
using Hermes.Api.Routes.Requests;

namespace Hermes.Api.Routes.Validations;

public class CreateRouteRequestValidator : AbstractValidator<CreateRouteRequest>
{
    public CreateRouteRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");


        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithState(_ => RouteValidationErrorCode.OperationTypeIdRequired)
            .WithMessage("Operation type ID is required.");

        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => RouteValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }


}

public class ActivateRouteRequestValidator : AbstractValidator<ActivateRouteRequest>
{
    public ActivateRouteRequestValidator()
    {
        RuleFor(x => x.RouteId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.RouteIdRequired)
        .WithMessage("Route ID is required.");
    }
}

public class DeactivateRouteRequestValidator : AbstractValidator<DeactivateRouteRequest>
{
    public DeactivateRouteRequestValidator()
    {
        RuleFor(x => x.RouteId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.RouteIdRequired)
        .WithMessage("Route ID is required.");
    }
}

public class DeleteRouteRequestValidator : AbstractValidator<DeleteRouteRequest>
{
    public DeleteRouteRequestValidator()
    {
        RuleFor(x => x.RouteId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.RouteIdRequired)
        .WithMessage("Route ID is required.");
    }
}

public class RestoreRouteRequestValidator : AbstractValidator<RestoreRouteRequest>
{
    public RestoreRouteRequestValidator()
    {
        RuleFor(x => x.RouteId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.RouteIdRequired)
        .WithMessage("Route ID is required.");
    }
}
