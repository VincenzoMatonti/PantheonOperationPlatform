using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;

namespace Athena.Api.Common.builders;

public class ApiResponseBuilder
{
    public static ApiResponse BuildSuccess()
    {
        return ApiResponse.Ok();
    }

    public static ApiResponse<T> BuildSuccess<T>(T data)
    {
        if (data is null) throw new ApiArgumentNullException(nameof(data));
        return ApiResponse<T>.Ok(data);
    }

    public static ApiResponse BuildFailure(IReadOnlyList<ApiErrorResponse> errors)
    {
        if (errors is null) throw new ApiArgumentNullException(nameof(errors));
        return ApiResponse.Fail(errors);
    }

    public static ApiResponse BuildFailure(ApiErrorType type, ApiErrorEntity entity, string code, string message)
    {
        return ApiResponse.Fail(type, entity, code, message);
    }
}