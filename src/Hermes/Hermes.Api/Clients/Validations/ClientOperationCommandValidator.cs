using FluentValidation;
using Hermes.Api.Clients.Requests;

namespace Hermes.Api.Clients.Validations;

public class CreateClientOperationRequestValidator : AbstractValidator<CreateClientOperationRequest>
{
    public CreateClientOperationRequestValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");
        RuleFor(x => x.OperationTypeId).NotEmpty().WithMessage("Operation type ID is required.");
    }
}

public class EnableClientOperationRequestValidator : AbstractValidator<EnableClientOperationRequest>
{
    public EnableClientOperationRequestValidator()
    {
        RuleFor(x => x.ClientOperationId).NotEmpty().WithMessage("Client operation ID is required.");
    }
}

public class DisableClientOperationRequestValidator : AbstractValidator<DisableClientOperationRequest>
{
    public DisableClientOperationRequestValidator()
    {
        RuleFor(x => x.ClientOperationId).NotEmpty().WithMessage("Client operation ID is required.");
    }
}

public class DeleteClientOperationRequestValidator : AbstractValidator<DeleteClientOperationRequest>
{
    public DeleteClientOperationRequestValidator()
    {
        RuleFor(x => x.ClientOperationId).NotEmpty().WithMessage("Client operation ID is required.");
    }
}

public class RestoreClientOperationRequestValidator : AbstractValidator<RestoreClientOperationRequest>
{
    public RestoreClientOperationRequestValidator()
    {
        RuleFor(x => x.ClientOperationId).NotEmpty().WithMessage("Client operation ID is required.");
    }
}