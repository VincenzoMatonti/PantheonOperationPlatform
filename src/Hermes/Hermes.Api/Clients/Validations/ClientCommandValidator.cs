using FluentValidation;
using Hermes.Api.Clients.Requests;

namespace Hermes.Api.Clients.Validations;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientCodeRequired)
            .WithMessage("Client code is required.")
            .MaximumLength(100)
            .WithState(_ => ClientValidationErrorCode.ClientCodeMaximumLength)
            .WithMessage("Client code cannot exceed 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientNameRequired)
            .WithMessage("Client name is required.")
            .MaximumLength(200)
            .WithState(_ => ClientValidationErrorCode.ClientNameMaximumLength)
            .WithMessage("Client name cannot exceed 200 characters.");
    }
}

public class RenameClientRequestValidator : AbstractValidator<RenameClientRequest>
{
    public RenameClientRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientNameRequired)
            .WithMessage("Client name is required.")
            .MaximumLength(200)
            .WithState(_ => ClientValidationErrorCode.ClientNameMaximumLength)
            .WithMessage("Client name cannot exceed 200 characters.");
    }
}

public class RenameClientCodeRequestValidator : AbstractValidator<RenameClientCodeRequest>
{
    public RenameClientCodeRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");

        RuleFor(x => x.NewCode)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientCodeRequired)
            .WithMessage("New client code is required.")
            .MaximumLength(100)
            .WithState(_ => ClientValidationErrorCode.ClientCodeMaximumLength)
            .WithMessage("Client code cannot exceed 100 characters.");
    }
}

public class ActivateClientRequestValidator : AbstractValidator<ActivateClientRequest>
{
    public ActivateClientRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");
    }
}

public class DeactivateClientRequestValidator : AbstractValidator<DeactivateClientRequest>
{
    public DeactivateClientRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");
    }
}

public class DeleteClientRequestValidator : AbstractValidator<DeleteClientRequest>
{
    public DeleteClientRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");
    }
}

public class RestoreClientRequestValidator : AbstractValidator<RestoreClientRequest>
{
    public RestoreClientRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");
    }
}