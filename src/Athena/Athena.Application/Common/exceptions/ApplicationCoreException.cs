namespace Athena.Application.Common.exceptions;

public class ApplicationCoreException(
    string code, ApplicationCoreErrorType errorType,
    string message, Exception innerException) : Exception(message, innerException)
{
    public string Code { get; } = code;
    public ApplicationCoreErrorType ErrorType { get; } = errorType;
}

public enum ApplicationCoreErrorType
{
    Validation,
    NotFound,
    Conflict,
    Internal
}