using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public class ApiArgumentException(string message, string? parameterName = null) : ApiCommonException(ApiCommonErrorCode.ArgumentInvalid, message)
{
    public string? ParameterName { get; } = parameterName;
}