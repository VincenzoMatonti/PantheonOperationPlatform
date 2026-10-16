using Athena.Api.Common.contracts;

namespace Athena.Api.Common.interfaces;

public interface IExceptionMapper
{
    bool CanHandle(Exception exception);
    ExceptionMappingResult Map(Exception exception);
}