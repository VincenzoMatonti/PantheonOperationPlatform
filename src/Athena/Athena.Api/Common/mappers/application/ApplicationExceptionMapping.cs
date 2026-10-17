using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Microsoft.AspNetCore.Http;

namespace Athena.Api.Common.mappers.application;

internal static class ApplicationExceptionMapping
{
    public static ExceptionMappingResult Create(int statusCode, ApiErrorEntity entity, string code, string message)
    {
        return new ExceptionMappingResult(statusCode,
        [
            new ApiErrorResponse
            {
               Type = statusCode switch
               {
                   StatusCodes.Status400BadRequest => ApiErrorType.Validation,
                   StatusCodes.Status404NotFound => ApiErrorType.NotFound,
                   StatusCodes.Status409Conflict => ApiErrorType.Conflict,
                   _ => ApiErrorType.Internal
               },
               Entity = entity,
               Code = code,
               Message = message
           }
        ]);
    }
}