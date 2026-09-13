namespace Hermes.Api.Common;

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

    public static ApiResponse Fail(ApiErrorType type, string code, string message)
    {
        return new ApiResponse
        {
            Success = false,
            Error = new ApiErrorResponse
            {
                Type = type,
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

    public static ApiResponse<T> Fail(ApiErrorType type, string code, string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = new ApiErrorResponse
            {
                Type = type,
                Code = code,
                Message = message
            }
        };
    }
}

public class ApiErrorResponse
{
    public ApiErrorType Type { get; set; }
    public string Code { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public enum ApiErrorType
{
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Internal = 6
}