
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Domain.SystemManagement.Exceptions;

namespace Athena.Api.Common.mappers.domain;

public sealed class SystemLifecycleDomainExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is SystemLifecycleException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not SystemLifecycleException domainException)
            throw new ApiArgumentException("Expected a SystemLifecycleException.", nameof(exception));

        var statusCode = domainException.Code switch
        {
            SystemLifecycleErrorCode.SystemIdRequired or SystemLifecycleErrorCode.InvalidDesiredState
                => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status409Conflict
        };

        return DomainExceptionMapping.Create(statusCode, ApiErrorEntity.System, domainException.Code.ToString(), domainException.Message);
    }
}
