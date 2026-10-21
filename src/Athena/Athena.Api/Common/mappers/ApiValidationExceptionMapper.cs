using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Microsoft.AspNetCore.Http;

namespace Athena.Api.Common.mappers;

public sealed class ApiValidationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is ApiValidationException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not ApiValidationException validationException)
            throw new ApiArgumentException("The exception is not an API validation exception.", nameof(exception));

        return new ExceptionMappingResult(StatusCodes.Status400BadRequest,
        [.. validationException.Errors.Select(error => new ApiErrorResponse
            {
                Type = ApiErrorType.Validation,
                Entity = ApiErrorEntity.Request,
                Code = "VALIDATION_ERROR",
                Message = string.IsNullOrWhiteSpace(error.PropertyName) ? error.ErrorMessage : $"{error.PropertyName}: {error.ErrorMessage}"
            })
        ]);
    }
}