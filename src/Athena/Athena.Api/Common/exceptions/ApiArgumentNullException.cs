using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public class ApiArgumentNullException(string parameterName) : ApiCommonException(
    ApiCommonErrorCode.ArgumentNull,
    $"Argument '{parameterName}' cannot be null.")
{
    public string ParameterName { get; } = parameterName;
}