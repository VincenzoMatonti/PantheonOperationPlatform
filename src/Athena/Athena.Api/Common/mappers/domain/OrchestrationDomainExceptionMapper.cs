
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Domain.OrchestrationRegistry.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Athena.Api.Common.mappers.domain;

public class OrchestrationDomainExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is OrchestrationException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not OrchestrationException domainException)
            throw new ApiArgumentException("Expected an OrchestrationException.", nameof(exception));

        var statusCode = domainException.Code switch
        {
            OrchestrationErrorCode.OrchestrationIdRequired
                or OrchestrationErrorCode.OrchestrationIdInvalid
                or OrchestrationErrorCode.OrchestrationNameRequired
                or OrchestrationErrorCode.OrchestrationNamesRequired
                or OrchestrationErrorCode.InvalidRegistrationStatus
                or OrchestrationErrorCode.InvalidAvailabilityStatus
                => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status409Conflict
        };

        return DomainExceptionMapping.Create(statusCode, ApiErrorEntity.Orchestration, domainException.Code.ToString(), domainException.Message);
    }
}
