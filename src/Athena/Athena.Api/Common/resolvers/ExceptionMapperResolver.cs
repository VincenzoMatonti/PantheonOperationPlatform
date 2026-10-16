using Athena.Api.Common.contracts;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;

namespace Athena.Api.Common.resolvers;

public sealed class ExceptionMapperResolver
{
    private readonly IReadOnlyList<IExceptionMapper> _mappers;

    public ExceptionMapperResolver(IEnumerable<IExceptionMapper> mappers)
    {
        if (mappers is null) throw new ApiArgumentNullException(nameof(mappers));
        _mappers = [.. mappers];
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is null) throw new ApiArgumentNullException(nameof(exception));
        var mapper = _mappers.FirstOrDefault(candidate => candidate.CanHandle(exception)) ??
        throw new ApiExceptionMapperNotFoundException(exception.GetType());
        return mapper.Map(exception);
    }
}