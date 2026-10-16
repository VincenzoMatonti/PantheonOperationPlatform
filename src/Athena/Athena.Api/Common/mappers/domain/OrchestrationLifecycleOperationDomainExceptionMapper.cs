
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Domain.OrchestrationManagement.Exceptions;

namespace Athena.Api.Common.mappers.domain;

public sealed class OrchestrationLifecycleOperationDomainExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is OrchestrationLifecycleOperationException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not OrchestrationLifecycleOperationException domainException)
            throw new ApiArgumentException("Expected an OrchestrationLifecycleOperationException.", nameof(exception));

        var statusCode = domainException.Code switch
        {
            OrchestrationLifecycleOperationErrorCode.IdRequired
                or OrchestrationLifecycleOperationErrorCode.OrchestrationIdRequired
                or OrchestrationLifecycleOperationErrorCode.ErrorMessageRequired
                or OrchestrationLifecycleOperationErrorCode.InvalidType
                or OrchestrationLifecycleOperationErrorCode.InvalidStatus
                => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status409Conflict
        };

        return DomainExceptionMapping.Create(statusCode, ApiErrorEntity.OrchestrationOperation, domainException.Code.ToString(), domainException.Message);
    }
}
