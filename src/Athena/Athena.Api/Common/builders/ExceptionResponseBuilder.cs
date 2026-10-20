using Athena.Api.Common.contracts;
using Athena.Api.Common.exceptions;

namespace Athena.Api.Common.builders;

public class ExceptionResponseBuilder
{
    public static ExceptionResponse Build(ExceptionMappingResult mappingResult)
    {
        if (mappingResult is null) throw new ApiArgumentNullException(nameof(mappingResult));
        var body = ApiResponseBuilder.BuildFailure(mappingResult.Errors);
        return new ExceptionResponse(mappingResult.StatusCode, body);
    }
}