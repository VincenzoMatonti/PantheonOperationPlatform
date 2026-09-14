namespace Hermes.Api.Exceptions.Builders;

public class ExceptionResponse(int statusCode, object body)
{
    public int StatusCode { get; } = statusCode;

    public object Body { get; } = body;
}