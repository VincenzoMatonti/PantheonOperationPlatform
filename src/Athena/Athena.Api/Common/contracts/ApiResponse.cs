using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;

namespace Athena.Api.Common.contracts;

public class ApiResponse
{
    public bool Success { get; init; }
    public IReadOnlyList<ApiErrorResponse> Errors { get; init; } = [];

    public static ApiResponse Ok()
    {
        return new ApiResponse
        {
            Success = true
        };
    }

    public static ApiResponse Fail(ApiErrorType type, ApiErrorEntity entity, string code, string message)
    {
        return Fail(
        [
            new ApiErrorResponse
            {
                Type = type,
                Entity = entity,
                Code = code,
                Message = message
            }
        ]);
    }

    public static ApiResponse Fail(IReadOnlyList<ApiErrorResponse> errors)
    {
        if (errors is null) throw new ApiArgumentNullException(nameof(errors));
        return new ApiResponse
        {
            Success = false,
            Errors = errors
        };
    }
}

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<ApiErrorResponse> Errors { get; init; } = [];

    public static ApiResponse<T> Ok(T data)
    {
        if (data is null) throw new ApiArgumentNullException(nameof(data));
        return new ApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(ApiErrorType type, ApiErrorEntity entity, string code, string message)
    {
        return Fail(
        [
            new ApiErrorResponse
            {
                Type = type,
                Entity = entity,
                Code = code,
                Message = message
            }
        ]);
    }

    public static ApiResponse<T> Fail(IReadOnlyList<ApiErrorResponse> errors)
    {
        if (errors is null) throw new ApiArgumentNullException(nameof(errors));
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}