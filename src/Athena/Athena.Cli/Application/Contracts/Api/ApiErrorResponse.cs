using System.Text.Json;

namespace Athena.Cli.Application.Contracts.Api;

public class ApiErrorResponse
{
    public JsonElement Type { get; init; }
    public JsonElement Entity { get; init; }
    public required string Code { get; init; }
    public required string Message { get; init; }
}