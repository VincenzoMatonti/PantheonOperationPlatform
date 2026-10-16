
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.interfaces;
using Athena.Domain.SystemRegistry.Exceptions;

namespace Athena.Api.Common.mappers.domain;

public sealed class SystemRegistryDomainExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception) => exception is SystemRegistryException;

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not SystemRegistryException domainException)
            throw new ArgumentException("Expected a SystemRegistryException.", nameof(exception));

        var statusCode = domainException.Code switch
        {
            SystemRegistryErrorCode.SystemIdRequired
                or SystemRegistryErrorCode.SystemNameRequired
                or SystemRegistryErrorCode.SystemNameTooLong
                or SystemRegistryErrorCode.SystemEndpointRequired
                or SystemRegistryErrorCode.SystemEndpointInvalid
                or SystemRegistryErrorCode.SystemCapabilityRequired
                => StatusCodes.Status400BadRequest,

            SystemRegistryErrorCode.SystemNotFound
                => StatusCodes.Status404NotFound,

            _ => StatusCodes.Status409Conflict
        };

        var entity = domainException.Code switch
        {
            SystemRegistryErrorCode.SystemEndpointRequired
                or SystemRegistryErrorCode.SystemEndpointInvalid
                or SystemRegistryErrorCode.SystemEndpointAlreadyExists
                => ApiErrorEntity.SystemEndpoint,

            SystemRegistryErrorCode.SystemCapabilityRequired
                or SystemRegistryErrorCode.SystemCapabilityAlreadyExists
                => ApiErrorEntity.SystemCapability,

            _ => ApiErrorEntity.System
        };

        return DomainExceptionMapping.Create(statusCode, entity, domainException.Code.ToString(), domainException.Message);
    }
}
