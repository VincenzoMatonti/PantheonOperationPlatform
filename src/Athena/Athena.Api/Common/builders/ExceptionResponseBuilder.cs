using Athena.Api.Common.contracts;
using Athena.Api.Common.exceptions;

namespace Athena.Api.Common.builders;

public class ExceptionResponseBuilder
{
    public static ExceptionResponse Build(ExceptionMappingResult mappingResult)
    {
        if (mappingResult is null) throw new ApiArgumentNullException(nameof(mappingResult));
        return new ExceptionResponse(mappingResult.StatusCode, ApiResponse.Fail(mappingResult.Errors));
    }
}