using Athena.Api.Common.enums;

namespace Athena.Api.Common.contracts;

public class ApiErrorResponse
{
    public ApiErrorType Type { get; init; }
    public ApiErrorEntity Entity { get; init; } = ApiErrorEntity.Unknown;
    public required string Code { get; init; }
    public required string Message { get; init; }
}