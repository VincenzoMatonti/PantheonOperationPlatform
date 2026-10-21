
using Athena.Api.Common.exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Athena.Api.Common.services;

public class RequestValidationService(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ApiArgumentNullException(nameof(serviceProvider));

    public async Task<ValidationResult> ValidateAsync(object request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ApiArgumentNullException(nameof(request));
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        if (_serviceProvider.GetService(validatorType) is not IValidator validator) return new ValidationResult();
        var context = new ValidationContext<object>(request);
        return await validator.ValidateAsync(context, cancellationToken);
    }
}
