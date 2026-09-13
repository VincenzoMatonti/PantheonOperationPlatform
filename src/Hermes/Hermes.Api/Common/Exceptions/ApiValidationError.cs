namespace Hermes.Api.Common.Exceptions;


public class ValidationError(string propertyName, Enum code, string message)
{
    public string PropertyName { get; } = propertyName;
    public Enum Code { get; } = code;
    public string Message { get; } = message;
}