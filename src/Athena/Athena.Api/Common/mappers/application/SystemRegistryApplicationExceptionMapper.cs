using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Application.SystemRegistry.Exceptions;

namespace Athena.Api.Common.mappers.application;

public class SystemRegistryApplicationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is SystemRegistryException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not SystemRegistryException applicationException)
            throw new ApiArgumentException("The exception is not an Athena Application SystemRegistry exception.", nameof(exception));

        var statusCode = applicationException.Code switch
        {
            SystemRegistryErrorCode.SystemNotFound => StatusCodes.Status404NotFound,
            SystemRegistryErrorCode.SystemNotFoundByName => StatusCodes.Status404NotFound,
            SystemRegistryErrorCode.SystemNameAlreadyExists => StatusCodes.Status409Conflict,
            _ => throw new ApiArgumentException($"Unsupported SystemRegistry application error code: '{applicationException.Code}'.", nameof(exception))
        };

        var message = applicationException.Code switch
        {
            SystemRegistryErrorCode.SystemNotFound => "The requested system was not found.",
            SystemRegistryErrorCode.SystemNotFoundByName => "The requested system was not found.",
            SystemRegistryErrorCode.SystemNameAlreadyExists => "A system with the specified name already exists.",
            _ => throw new ApiArgumentException($"Unsupported SystemRegistry application error code: '{applicationException.Code}'.", nameof(exception))
        };

        return ApplicationExceptionMapping.Create(statusCode, ApiErrorEntity.System, applicationException.Code.ToString(), message);
    }

    ExceptionMappingResult IExceptionMapper.Map(Exception exception)
    {
        throw new NotImplementedException();
    }
}