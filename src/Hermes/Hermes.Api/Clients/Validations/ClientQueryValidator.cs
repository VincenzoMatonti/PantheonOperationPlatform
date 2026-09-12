using FluentValidation;
using Hermes.Api.Clients.Requests;

namespace Hermes.Api.Clients.Validations;

public class GetClientByIdRequestValidator : AbstractValidator<GetClientByIdRequest>
{
    public GetClientByIdRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
    }
}

public class GetClientByCodeRequestValidator : AbstractValidator<GetClientByCodeRequest>
{
    public GetClientByCodeRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Client code is required.")
            .MaximumLength(100).WithMessage("Client code cannot exceed 100 characters.");
    }
}

public class GetClientCodeByIdRequestValidator : AbstractValidator<GetClientCodeByIdRequest>
{
    public GetClientCodeByIdRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
    }
}
