using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public class ApiInvalidExceptionMappingStatusCodeException(int statusCode)
        : ApiCommonException(
        ApiCommonErrorCode.InvalidExceptionMappingStatusCode,
        $"HTTP status code '{statusCode}' is invalid for an exception mapping.")
{
    public int StatusCode { get; } = statusCode;
}