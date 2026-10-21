using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public class ApiExceptionMapperNotFoundException(Type exceptionType) : ApiCommonException(
        ApiCommonErrorCode.ExceptionMapperNotFound,
        $"No API exception mapper is registered for '{exceptionType.FullName}'.")
{
    public Type ExceptionType { get; } = exceptionType;
}