using Hermes.Api.Exceptions.Models;

namespace Hermes.Api.Exceptions.Mappings;

public class ExceptionMapperResolver(IEnumerable<IExceptionMapper> mappers)
{
    private readonly IEnumerable<IExceptionMapper> _mappers = mappers;

    public IExceptionMapper Resolve(Exception exception)
    {
        var mapper = _mappers.FirstOrDefault(mapper => mapper.CanHandle(exception));
        if (mapper != null) return mapper;
        throw new ApiExceptionMapperNotFoundException(exception.GetType().Name);
    }
}