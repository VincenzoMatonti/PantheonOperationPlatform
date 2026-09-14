namespace Hermes.Api.Exceptions.Mappings;

public interface IExceptionMapper
{
    bool CanHandle(Exception exception);

    ExceptionMappingResult Map(Exception exception);
}