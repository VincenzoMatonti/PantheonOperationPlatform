namespace Athena.Cli.Application.Contracts.Api;

public class ApiResponse<T>
{
    public bool Success { get; init; }

    public T? Data { get; init; }

    public IReadOnlyList<ApiErrorResponse> Errors { get; init; } = [];
}

public class ApiResponse
{
    public bool Success { get; init; }

    public IReadOnlyList<ApiErrorResponse> Errors { get; init; } = [];
}