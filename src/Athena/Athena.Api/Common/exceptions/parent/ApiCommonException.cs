using Athena.Api.Common.enums;

namespace Athena.Api.Common.exceptions.parent;

public abstract class ApiCommonException : Exception
{
    public ApiCommonErrorCode Code { get; }
    protected ApiCommonException(ApiCommonErrorCode code, string message) : base(message)
    {
        Code = code;
    }

    protected ApiCommonException(ApiCommonErrorCode code, string message, Exception innerException) : base(message, innerException)
    {
        Code = code;
    }
}