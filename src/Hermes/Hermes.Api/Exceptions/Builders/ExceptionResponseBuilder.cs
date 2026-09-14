using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;

namespace Hermes.Api.Exceptions.Builders;

public class ExceptionResponseBuilder
{
    public ExceptionResponse Build(ExceptionMappingResult result)
    {
        var response = ApiResponse.Fail(result.Errors);

        return new ExceptionResponse(result.StatusCode, response);
    }
}