using Athena.Application.Common.exceptions;
using Athena.Application.Common.Interfaces;
using Athena.Domain.Common;

namespace Athena.Application.Common.Mappings;

public class DomainExceptionMapper : IApplicationExceptionMapper
{
    public bool CanMap(Exception exception)
    {
        return exception is DomainException;
    }

    public ApplicationCoreException Map(Exception exception)
    {
        if (exception is not DomainException domainException)
            throw new ArgumentException("L'eccezione non è un'eccezione di dominio.", nameof(exception));

        return new ApplicationCoreException(
            domainException.CodeName,
            MapErrorType(domainException.ErrorType),
            domainException.Message, domainException);
    }

    private static ApplicationCoreErrorType MapErrorType(DomainErrorType errorType)
    {
        return errorType switch
        {
            DomainErrorType.Validation => ApplicationCoreErrorType.Validation,
            DomainErrorType.NotFound => ApplicationCoreErrorType.NotFound,
            DomainErrorType.Conflict => ApplicationCoreErrorType.Conflict,
            DomainErrorType.Internal => ApplicationCoreErrorType.Internal,
            _ => ApplicationCoreErrorType.Internal
        };
    }
}