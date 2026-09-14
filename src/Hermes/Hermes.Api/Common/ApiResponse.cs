namespace Hermes.Api.Common;

public class ApiResponse
{
    public bool Success { get; set; }
    public List<ApiErrorResponse>? Errors { get; set; }

    public static ApiResponse Ok()
    {
        return new ApiResponse
        {
            Success = true
        };
    }

    public static ApiResponse Fail(ApiErrorType type, ApiErrorEntity entity, string code, string message)
    {
        return new ApiResponse
        {
            Success = false,
            Errors = [
                new ApiErrorResponse
                {
                    Type = type,
                    Entity = entity,
                    Code = code,
                    Message = message
                }
            ]
        };
    }

    public static ApiResponse Fail(List<ApiErrorResponse> errors)
    {
        return new ApiResponse
        {
            Success = false,
            Errors = errors
        };
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<ApiErrorResponse>? Errors { get; set; }
    public static ApiResponse<T> Ok(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(ApiErrorType type, ApiErrorEntity entity, string code, string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = [
                new ApiErrorResponse
                {
                    Type = type,
                    Entity = entity,
                    Code = code,
                    Message = message
                }
            ]
        };
    }

    public static ApiResponse<T> Fail(List<ApiErrorResponse> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}

public class ApiErrorResponse
{
    public ApiErrorType Type { get; set; }
    public ApiErrorEntity Entity { get; set; }
    public string Code { get; set; } = null!;
    public string Message { get; set; } = null!;
}