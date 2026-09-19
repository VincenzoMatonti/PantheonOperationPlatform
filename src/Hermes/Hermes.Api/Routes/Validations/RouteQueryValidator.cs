using FluentValidation;
using Hermes.Api.Routes.Requests;

namespace Hermes.Api.Routes.Validations;

public class GetRouteByIdRequestValidator : AbstractValidator<GetRouteByIdRequest>
{
    public GetRouteByIdRequestValidator()
    {
        RuleFor(x => x.RouteId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.RouteIdRequired)
        .WithMessage("Route ID is required.");
    }
}

public class GetRouteRequestValidator : AbstractValidator<GetRouteRequest>
{
    public GetRouteRequestValidator()
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

public class GetRoutesByClientIdRequestValidator : AbstractValidator<GetRoutesByClientIdRequest>
{
    public GetRoutesByClientIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
        .WithMessage("Client ID is required.");
    }
}

public class GetRoutesByOperationTypeIdRequestValidator : AbstractValidator<GetRoutesByOperationTypeIdRequest>
{
    public GetRoutesByOperationTypeIdRequestValidator()
    {
        RuleFor(x => x.OperationTypeId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.OperationTypeIdRequired)
        .WithMessage("Operation type ID is required.");
    }
}

public class GetRoutesByEndpointIdRequestValidator : AbstractValidator<GetRoutesByEndpointIdRequest>
{
    public GetRoutesByEndpointIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.EndpointIdRequired)
        .WithMessage("Endpoint ID is required.");
    }
}

public class GetRoutesByClientIdAndOperationTypeIdRequestValidator : AbstractValidator<GetRoutesByClientIdAndOperationTypeIdRequest>
{
    public GetRoutesByClientIdAndOperationTypeIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
        .WithMessage("Client ID is required.");


        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithState(_ => RouteValidationErrorCode.OperationTypeIdRequired)
            .WithMessage("Operation type ID is required.");
    }


}

public class GetRoutesByClientIdAndEndpointIdRequestValidator : AbstractValidator<GetRoutesByClientIdAndEndpointIdRequest>
{
    public GetRoutesByClientIdAndEndpointIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
        .WithMessage("Client ID is required.");


        RuleFor(x => x.EndpointId)
            .NotEmpty()
            .WithState(_ => RouteValidationErrorCode.EndpointIdRequired)
            .WithMessage("Endpoint ID is required.");
    }

}

public class GetRoutesByOperationTypeIdAndEndpointIdRequestValidator : AbstractValidator<GetRoutesByOperationTypeIdAndEndpointIdRequest>
{
    public GetRoutesByOperationTypeIdAndEndpointIdRequestValidator()
    {
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

public class GetActiveRoutesByClientIdRequestValidator : AbstractValidator<GetActiveRoutesByClientIdRequest>
{
    public GetActiveRoutesByClientIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
        .WithMessage("Client ID is required.");
    }
}

public class GetNonActiveRoutesByClientIdRequestValidator : AbstractValidator<GetNonActiveRoutesByClientIdRequest>
{
    public GetNonActiveRoutesByClientIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
        .WithMessage("Client ID is required.");
    }
}

public class GetDeletedRoutesByClientIdRequestValidator : AbstractValidator<GetDeletedRoutesByClientIdRequest>
{
    public GetDeletedRoutesByClientIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.ClientIdRequired)
        .WithMessage("Client ID is required.");
    }
}

public class GetActiveRoutesByOperationTypeIdRequestValidator : AbstractValidator<GetActiveRoutesByOperationTypeIdRequest>
{
    public GetActiveRoutesByOperationTypeIdRequestValidator()
    {
        RuleFor(x => x.OperationTypeId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.OperationTypeIdRequired)
        .WithMessage("Operation type ID is required.");
    }
}

public class GetNonActiveRoutesByOperationTypeIdRequestValidator : AbstractValidator<GetNonActiveRoutesByOperationTypeIdRequest>
{
    public GetNonActiveRoutesByOperationTypeIdRequestValidator()
    {
        RuleFor(x => x.OperationTypeId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.OperationTypeIdRequired)
        .WithMessage("Operation type ID is required.");
    }
}

public class GetDeletedRoutesByOperationTypeIdRequestValidator : AbstractValidator<GetDeletedRoutesByOperationTypeIdRequest>
{
    public GetDeletedRoutesByOperationTypeIdRequestValidator()
    {
        RuleFor(x => x.OperationTypeId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.OperationTypeIdRequired)
        .WithMessage("Operation type ID is required.");
    }
}

public class GetActiveRoutesByEndpointIdRequestValidator : AbstractValidator<GetActiveRoutesByEndpointIdRequest>
{
    public GetActiveRoutesByEndpointIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.EndpointIdRequired)
        .WithMessage("Endpoint ID is required.");
    }
}

public class GetNonActiveRoutesByEndpointIdRequestValidator : AbstractValidator<GetNonActiveRoutesByEndpointIdRequest>
{
    public GetNonActiveRoutesByEndpointIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.EndpointIdRequired)
        .WithMessage("Endpoint ID is required.");
    }
}

public class GetDeletedRoutesByEndpointIdRequestValidator : AbstractValidator<GetDeletedRoutesByEndpointIdRequest>
{
    public GetDeletedRoutesByEndpointIdRequestValidator()
    {
        RuleFor(x => x.EndpointId)
        .NotEmpty()
        .WithState(_ => RouteValidationErrorCode.EndpointIdRequired)
        .WithMessage("Endpoint ID is required.");
    }
}
