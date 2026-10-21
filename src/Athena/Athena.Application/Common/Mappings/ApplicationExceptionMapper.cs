using Athena.Application.Common.exceptions;
using Athena.Application.Common.Interfaces;
using ApplicationException = Athena.Application.Common.exceptions.ApplicationException;

namespace Athena.Application.Common.Mappings;

public class ApplicationExceptionMapper : IApplicationExceptionMapper
{
    public bool CanMap(Exception exception)
    {
        return exception is ApplicationException;
    }

    public ApplicationCoreException Map(Exception exception)
    {
        if (exception is not ApplicationException applicationException)
            throw new ArgumentException("L'eccezione non è un'eccezione applicativa.", nameof(exception));

        return new ApplicationCoreException(
            applicationException.CodeName,
            applicationException.ErrorType,
            applicationException.Message,
            applicationException);
    }
}