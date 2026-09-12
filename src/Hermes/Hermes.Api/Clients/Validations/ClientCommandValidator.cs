using FluentValidation;
using Hermes.Api.Clients.Requests;

namespace Hermes.Api.Clients.Validations;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Client code is required.")
            .MaximumLength(100).WithMessage("Client code cannot exceed 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .MaximumLength(200).WithMessage("Client name cannot exceed 200 characters.");
    }
}

public class RenameClientRequestValidator : AbstractValidator<RenameClientRequest>
{
    public RenameClientRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .MaximumLength(200).WithMessage("Client name cannot exceed 200 characters.");
    }
}

public class RenameClientCodeRequestValidator : AbstractValidator<RenameClientCodeRequest>
{
    public RenameClientCodeRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required.");

        RuleFor(x => x.NewCode)
            .NotEmpty().WithMessage("New client code is required.")
            .MaximumLength(100).WithMessage("Client code cannot exceed 100 characters.");
    }
}

public class ActivateClientRequestValidator : AbstractValidator<ActivateClientRequest>
{
    public ActivateClientRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
    }
}

public class DeactivateClientRequestValidator : AbstractValidator<DeactivateClientRequest>
{
    public DeactivateClientRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
    }
}

public class DeleteClientRequestValidator : AbstractValidator<DeleteClientRequest>
{
    public DeleteClientRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
    }
}

public class RestoreClientRequestValidator : AbstractValidator<RestoreClientRequest>
{
    public RestoreClientRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
    }
}
