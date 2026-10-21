using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public class ApiArgumentOutOfRangeException(string parameterName, object? actualValue, string message) :
    ApiCommonException(ApiCommonErrorCode.ArgumentOutOfRange, message)
{
    public string ParameterName { get; } = parameterName;
    public object? ActualValue { get; } = actualValue;
}