
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Domain.OrchestrationManagement.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Athena.Api.Common.mappers.domain;

public sealed class OrchestrationLifecycleDomainExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is OrchestrationLifecycleException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not OrchestrationLifecycleException domainException)
            throw new ApiArgumentException("Expected an OrchestrationLifecycleException.", nameof(exception));

        var statusCode = domainException.Code switch
        {
            OrchestrationLifecycleErrorCode.OrchestrationIdRequired
                or OrchestrationLifecycleErrorCode.InvalidDesiredState
                or OrchestrationLifecycleErrorCode.InvalidOperationalStatus
                => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status409Conflict
        };

        return DomainExceptionMapping.Create(statusCode, ApiErrorEntity.Orchestration, domainException.Code.ToString(), domainException.Message);
    }
}
