using Athena.Application.Common.exceptions;

namespace Athena.Application.Common.Interfaces;

public interface IApplicationExceptionMapper
{
    bool CanMap(Exception exception);

    ApplicationCoreException Map(Exception exception);
}