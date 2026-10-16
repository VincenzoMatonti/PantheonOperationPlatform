
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Domain.SystemManagement.Exceptions;

namespace Athena.Api.Common.mappers.domain;

public sealed class SystemLifecycleOperationDomainExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is SystemLifecycleOperationException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not SystemLifecycleOperationException domainException)
            throw new ApiArgumentException("Expected a SystemLifecycleOperationException.", nameof(exception));

        var statusCode = domainException.Code switch
        {
            SystemLifecycleOperationErrorCode.SystemIdRequired
                or SystemLifecycleOperationErrorCode.InvalidOperationType
                or SystemLifecycleOperationErrorCode.ErrorMessageRequired
                => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status409Conflict
        };

        return DomainExceptionMapping.Create(statusCode, ApiErrorEntity.System, domainException.Code.ToString(), domainException.Message);
    }
}
