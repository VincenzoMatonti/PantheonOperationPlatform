namespace Hermes.Api.Common.Responses;

public class ApiResponse
{
    public bool Success { get; set; }
    public ApiErrorResponse? Error { get; set; }
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
            Error = new ApiErrorResponse
            {
                Type = type,
                Entity = entity,
                Code = code,
                Message = message
            }
        };
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ApiErrorResponse? Error { get; set; }
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
            Error = new ApiErrorResponse
            {
                Type = type,
                Entity = entity,
                Code = code,
                Message = message
            }
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
